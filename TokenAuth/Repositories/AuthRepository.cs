using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using TokenAuth.Models;
using TokenAuth.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using TokenAuth.Utils;
using System.IO;

namespace TokenAuth.Repositories
{
    public interface IAuthRepository
    {
        AuthResponse GetToken(string userId, string password);
        AuthResponse RenewToken(string authHeader);
        TokenPayLoad DecodeToken(string authHeader);
        AuthResponse RenewToken(TokenPayLoad tokenPayLoad);
    }

    public class AuthRepository : IAuthRepository
    {
        static TokenAuthOptions _tokenOptions;
        readonly ILogger _logger;
        AuthContext _context;
        IConfigurationRoot _configuration { get; }
        int? tokenLifeTime;

        
        public AuthRepository(AuthContext context, ILogger<AuthRepository> logger, 
            TokenAuthOptions tokenOptions, IConfigurationRoot configuration)
        {
            _tokenOptions = tokenOptions;
            _logger = logger;
            _context = context;
            _configuration = configuration;

            var authSettings = _configuration.GetSection("Authentication");
            tokenLifeTime = authSettings.GetValue<int>("TokenLifeTime");
            if (!tokenLifeTime.HasValue || tokenLifeTime.Value <= 0)
            {
                tokenLifeTime = 10;
            }
        }

        private void LogApplicationMsg(string type, string message, string module, 
            string strguid="", int code=0)
        {
            try
            {
                _logger.LogInformation("LogApplicationMsg, " + message);

                _context.Database.ExecuteSqlQuery(@"exec [LOGS].[SP_LogApplicationMsg] @Type='" +
                type + "', @Message='" + message + "', @Module='" + module + "', @strguid='" + strguid +
                "', @CountOrCode=" + code);
            }
            catch (Exception ex)
            {
                _logger.LogError("LogApplicationMsg Failed. " + ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }
        }

        private void AddUserLoginAttempt(string userId, string userEmail, string token, bool isLoginSuccessful,
            string module, int roleId = 0, string paramsVal = "", int isTokenRenewalRequest=0,
            bool isLogoff = false)
        {
            try
            {
                _logger.LogInformation("User Login Attempt, uid = {0}, isLoginSuccessful = {1}",
                userId, isLoginSuccessful);

                _context.Database.ExecuteSqlQuery(@"exec [AUTH].[SP_AddUserLoginAttempt] @UserID='" + userId +
                    "', @Token='" + token + "', @IsLoginSuccess= " + isLoginSuccessful + ", @Module='" + module +
                    "', @Params='" + paramsVal + "', @IsTokenRenewalRequest=" + isTokenRenewalRequest +
                    ", @IsLogOff=" + isLogoff + ", @RoleID=" + roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddUserLoginAttempt Failed. " + ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }
        }

        private string GetCurrentDateTime()
        {
            return
                DateTime.Now.Year.ToString() + "-" +
                DateTime.Now.Month.ToString() + "-" +
                DateTime.Now.Day.ToString() + " " +
                DateTime.Now.Hour.ToString() + ":" +
                DateTime.Now.Minute.ToString() + ":" +
                DateTime.Now.Second.ToString();
        }

        private DateTime ConvertStringToDateTime(string strDateTime)
        {
            DateTime convertedDT;

            string datePart = strDateTime.Split(" ")[0];
            int year, month, day;

            year = System.Convert.ToInt32(datePart.Split("-")[0]);
            month = System.Convert.ToInt32(datePart.Split("-")[1]);
            day = System.Convert.ToInt32(datePart.Split("-")[2]);

            string timePart = strDateTime.Split(" ")[1];
            int hour, min, sec;

            hour = System.Convert.ToInt32(timePart.Split(":")[0]);
            min = System.Convert.ToInt32(timePart.Split(":")[1]);
            sec = System.Convert.ToInt32(timePart.Split(":")[2]);

            convertedDT = new DateTime(year, month, day, hour, min, sec);

            return convertedDT;

        }

        /// <summary>
        /// Check if Account is temporarily Locked
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool CheckAccountLocked(string userId, bool isSuccessAttempt)
        {
            bool isAccountLocked = false;

            try
            {
                int maxFailureLoginAttempts = _configuration.GetValue<int>("MaxFailureLoginAttempts");
                int lockAccountPeriodInMin = _configuration.GetValue<int>("LockAccountPeriodInMin");

                string loginFailuresFolderRelPath = _configuration.GetValue<string>("LoginFailuresFolderRelPath");
                string txtFileForUser = loginFailuresFolderRelPath.Replace("{UserName}", userId);

                //Check if json file for failure attempts exists for current user
                bool isFileExist = System.IO.File.Exists(txtFileForUser);


                if (isSuccessAttempt)
                {
                    if (isFileExist)
                    {
                        // Check if Lock Period expired
                        int failureAttempts = 0;
                        DateTime lastFailureDateTime;
                        string failureLine;
                        using (TextReader reader = File.OpenText(txtFileForUser))
                        {
                            failureLine = reader.ReadLine();
                            failureAttempts = System.Convert.ToInt32(failureLine.Split("~")[0]);
                            lastFailureDateTime = ConvertStringToDateTime(failureLine.Split("~")[1]);

                            TimeSpan span = DateTime.Now.Subtract(lastFailureDateTime);

                            if (failureAttempts >= maxFailureLoginAttempts && span.TotalMinutes < lockAccountPeriodInMin)
                            {
                                isAccountLocked = true;
                            }

                            reader.Close();
                        }
                        if (!isAccountLocked)
                        {
                            System.IO.File.Delete(txtFileForUser);
                        }
                    }

                }
                else
                {
                    if (isFileExist)
                    {
                        int failureAttempts = 0;
                        //DateTime lastFailureDateTime;
                        string failureLine;
                        using (TextReader reader = File.OpenText(txtFileForUser))
                        {
                            failureLine = reader.ReadLine();
                            failureAttempts = System.Convert.ToInt32(failureLine.Split("~")[0]);
                            //lastFailureDateTime = ConvertStringToDateTime(failureLine.Split("~")[1]);

                            failureAttempts++;
                            if (failureAttempts >= maxFailureLoginAttempts)
                            {
                                isAccountLocked = true;
                            }
                            reader.Close();
                        }

                        System.IO.File.WriteAllText(txtFileForUser, failureAttempts.ToString() + "~" + GetCurrentDateTime());
                    }
                    else
                    {
                        System.IO.File.WriteAllText(txtFileForUser, "1~" + GetCurrentDateTime());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method Name: {CheckAccountLocked}" + ex.Message);
            }

            return isAccountLocked;
        }

        public AuthResponse GetToken(string userId, string password)
        {
            var response = new AuthResponse();
            
            if (string.IsNullOrEmpty(userId))
            {
                return response;
            }
            
            string userEmail = "", userDisplayName = "";
            string personId = "", phone = "";
            string isActive = "";
            bool hasPic = false;
            int hasPicValue = 0;
            string picFileName = "";
            bool isAppAdmin = false;
            int isAppAdminValue = 0;
            string adminOnChurches = "";

            int roleId = 0;
            // This section checks for a mock user in the database
            try
            {
                //Check if user is in database
                //-----------------------------
                
                string sqlQuery = String.Format("exec [AUTH].[SP_AuthenticateUser] " +
                    "@UserID='{0}', @Password='{1}'", userId, password);
                var dr = _context.Database.ExecuteSqlQuery(sqlQuery);
                dr.DbDataReader.Read();
                var authenticated = dr.DbDataReader["IsAuthenticated"].ToString();
                
                // var authenticated = "1";

                if (authenticated == "1")
                {
                    if (CheckAccountLocked(userId, true))
                    {
                        int lockAccountPeriodInMin = _configuration.GetValue<int>("LockAccountPeriodInMin");

                        response.IsAuth = false;
                        response.Message = "Account is still locked. Please, try again after " + lockAccountPeriodInMin.ToString() + " minutes!";
                    }
                    else
                    {

                        personId = dr.DbDataReader["PersonId"].ToString();
                        userEmail = dr.DbDataReader["Email"].ToString();
                        userDisplayName = dr.DbDataReader["DisplayName"].ToString();
                        phone = dr.DbDataReader["Phone"].ToString();
                        isActive = dr.DbDataReader["IsActive"].ToString();
                        hasPicValue = System.Convert.ToInt32(dr.DbDataReader["HasPic"]);
                        isAppAdminValue = System.Convert.ToInt32(dr.DbDataReader["IsAppAdmin"]);
                        adminOnChurches = dr.DbDataReader["AdminOnChurches"].ToString();

                        hasPic = (hasPicValue == 1 ? true : false);

                        picFileName = dr.DbDataReader["PicFileName"].ToString();

                        //userEmail = "csdmichael@gmail.com";
                        // userDisplayName = "Michael Yaacoub";
                        // isActive = "0";

                        string role = (isActive == "0" ? "InactiveUser": "ActiveUser");

                        isAppAdmin = (isAppAdminValue == 1 ? true : false);

                        TokenInfo token = CreateToken(personId, userEmail, role, phone, userDisplayName, hasPic, picFileName, isAppAdmin, adminOnChurches);
                        AddUserLoginAttempt(userId, "", token.Token, true, "DB LOGIN", roleId);
                        return new AuthResponse
                        {
                            IsAuth = true,
                            ACCESS_TOKEN = token.Token,
                            EXPIRES_IN = (DateTime) token.Expires,
                            DisplayName = userDisplayName,
                            Phone = phone,
                            Email = userEmail,
                            Role = role,
                            PersonId = personId,
                            Message = "Success",
                            HasPic = hasPic,
                            PicFileName = picFileName,
                            IsAppAdmin = isAppAdmin,
                            AdminOnChurches = adminOnChurches
                        };
                    }
                }
                else
                {
                    bool isAccountLocked = CheckAccountLocked(userId, false);

                    response.IsAuth = false;

                    if (isAccountLocked)
                    {
                        response.Message = "Invalid userId or password - Account has been Locked!";
                    }
                    else
                    {
                        response.Message = "Invalid userId or password!";
                    }

                }
                // dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("DB Login Failed with Exception: " + ex.Message);
                _logger.LogDebug(ex.StackTrace);
                LogApplicationMsg("Error", ex.Message, "DB LOGIN", string.Empty, ex.HResult);
            }
            
            return response;
        }

        private int GetRoleId(string membership)
        {
            int roleId = 0;
            try
            {
                var sqlQuery = "exec [AUTH].[SP_UserRole_Get] @GroupNames = '" + membership + "'";
                var dr = _context.Database.ExecuteSqlQuery(sqlQuery);
                dr.DbDataReader.Read();
                Int32.TryParse(dr.DbDataReader["RoleId"].ToString(), out roleId);
                return roleId;
            }
            catch (Exception ex)
            {
                var errorMsg = "getRoleId Failed. " + ex.Message;
                _logger.LogError(errorMsg);
                _logger.LogDebug(ex.StackTrace);
                LogApplicationMsg("ERROR", errorMsg, "DB LOGIN", null, ex.HResult);
                return roleId;
            }
        }

        private TokenInfo CreateToken(string personId, string email, string role, string phone, string displayName, bool hasPic, string picFileName, bool isAppAdmin, string adminOnChurches)
        {
            TokenInfo tokenInfo = new TokenInfo();
            DateTime? expires = DateTime.UtcNow.AddMinutes(tokenLifeTime.Value);

            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(personId, "TokenAuth"),
                new[] 
                {
                    new Claim(ClaimTypes.Email, email, ClaimValueTypes.Email),
                    new Claim(ClaimTypes.Role, role, ClaimValueTypes.String),
                    new Claim("phone", phone, ClaimValueTypes.String),
                    new Claim("displayName", displayName, ClaimValueTypes.String),
                    new Claim("hasPic", hasPic.ToString(), ClaimValueTypes.Boolean),
                    new Claim("picFileName", picFileName.ToString(), ClaimValueTypes.String),
                    new Claim("isAppAdmin", isAppAdmin.ToString(), ClaimValueTypes.Boolean),
                    new Claim("adminOnChurches", adminOnChurches.ToString(), ClaimValueTypes.String)

                });

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {                
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                SigningCredentials = _tokenOptions.SigningCredentials,
                Subject = identity,
                Expires = expires
            });

            tokenInfo.Token = handler.WriteToken(securityToken);
            tokenInfo.Expires = expires;

            return tokenInfo;
        }

        public AuthResponse RenewToken(TokenPayLoad tokenPayLoad)
        {
            var newToken = CreateToken(tokenPayLoad.PersonId, tokenPayLoad.Email, tokenPayLoad.Role, tokenPayLoad.Phone, tokenPayLoad.DisplayName, tokenPayLoad.HasPic, tokenPayLoad.PicFileName, tokenPayLoad.IsAppAdmin, tokenPayLoad.AdminOnChurches);
            AddUserLoginAttempt(tokenPayLoad.PersonId, tokenPayLoad.Email, newToken.ToString(), true, "DB LOGIN");

            return new AuthResponse
            {
                IsAuth = tokenPayLoad.IsAuth,
                ACCESS_TOKEN = newToken.Token,
                PersonId = tokenPayLoad.PersonId,
                Email = tokenPayLoad.Email,
                Phone = tokenPayLoad.Phone,
                DisplayName = tokenPayLoad.DisplayName,
                Role = tokenPayLoad.Role,
                Message = tokenPayLoad.Message,
                HasPic = tokenPayLoad.HasPic,
                PicFileName = tokenPayLoad.PicFileName,
                IsAppAdmin = tokenPayLoad.IsAppAdmin,
                AdminOnChurches = tokenPayLoad.AdminOnChurches
            };
        }

        public AuthResponse RenewToken(string authHeader)
        {
            var tokenPayLoad = DecodeToken(authHeader);
            return RenewToken(tokenPayLoad);
        }

        public TokenPayLoad DecodeToken(string authHeader)
        {
            var authBits = authHeader.ToString().Split(' ');
            if (authBits.Length != 2)
            {
                _logger.LogError("Error: auth bits needs to be length 2");
                return new AuthResponse
                {
                    IsAuth = false,
                    Message = "Can't parse authorization header"
                };
            }
            if (!authBits[0].ToLowerInvariant().Equals("bearer"))
            {
                _logger.LogError("Error: authBits[0] must be bearer");
                return new AuthResponse
                {
                    IsAuth = false,
                    Message = "Can't parse authorization header"
                };
            }
            var handler = new JwtSecurityTokenHandler();
            var jst = handler.ReadJwtToken(authBits[1]);
            var personId = jst.Payload["unique_name"].ToString();
            
            string email = jst.Payload["email"].ToString();
            string role = jst.Payload["role"].ToString();
            string phone = "";
            string displayName = "";
            bool hasPic = false;
            string picFileName = "";
            bool isAppAdmin = false;
            string adminOnChurches = "";

            if (jst.Payload["phone"] != null)
                phone = jst.Payload["phone"].ToString();

            if (jst.Payload["displayName"] != null)
                displayName = jst.Payload["displayName"].ToString();

            if (jst.Payload["hasPic"] != null)
                hasPic = System.Convert.ToBoolean(jst.Payload["hasPic"]);

            if (jst.Payload["isAppAdmin"] != null)
                isAppAdmin = System.Convert.ToBoolean(jst.Payload["isAppAdmin"]);

            if (jst.Payload["picFileName"] != null)
                picFileName = jst.Payload["picFileName"].ToString().ToLower();

            if (jst.Payload["adminOnChurches"] != null)
                adminOnChurches = jst.Payload["adminOnChurches"].ToString().ToLower();

            // Maybe we will want to check if the token is still valid instead of relying on the client
            TokenInfo newToken = CreateToken(personId, email, role, phone, displayName, hasPic, picFileName, isAppAdmin, adminOnChurches);

            return new TokenPayLoad
            {
                IsAuth = true,
                PersonId = personId,
                Role = role,
                Email = email,
                Phone = phone,
                Message = "Success",
                HasPic = hasPic,
                DisplayName = displayName,
                PicFileName = picFileName,
                IsAppAdmin = isAppAdmin,
                AdminOnChurches = adminOnChurches

            };
        }

    }
}
