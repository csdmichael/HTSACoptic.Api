using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
using HTSA.API.Models;
using TokenAuth.Repositories;
using TokenAuth.Models;
using HTSA.DAL.Models;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        ILogger _logger;
        ApplContext _context;
        IAuthRepository _authRepository;
        INotifRepository _notifRepository;

        public PersonRepository(ApplContext context, ILogger<PersonRepository> logger, IAuthRepository authRepository, INotifRepository notifRepository)
        {
            _logger = logger;
            _context = context;
            _authRepository = authRepository;
            _notifRepository = notifRepository;
        }



        public UpdateResponse AddPerson(string baseURL, RegisteredUser pers, string activationCode)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {

                string sqlQuery;

                int paramCnt = 0;

                sqlQuery = @"exec PERS.[SP_Person_Add]";

                if (pers.RegPerson != null && pers.RegPerson.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @RegPerson = '" + pers.RegPerson + "'";
                    paramCnt++;
                }

                if (pers.FirstName != null && pers.FirstName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @FirstName = '" + pers.FirstName.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.MiddleName != null && pers.MiddleName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @MiddleName = '" + pers.MiddleName.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.LastName != null && pers.LastName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @LastName = '" + pers.LastName.Replace("'", "''") + "'";
                    paramCnt++;
                }

                //if (pers.MiddleName != null && pers.MiddleName.Trim() != "")
                //{
                //    if (paramCnt > 0)
                //        sqlQuery += ", ";
                //    sqlQuery += " @MiddleName = '" + pers.MiddleName + "'";
                //    paramCnt++;
                //}

                if (pers.Gender != null && pers.Gender.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Sex = '" + pers.Gender + "'";
                    paramCnt++;
                }

                if (pers.Email != null && pers.Email.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Email = '" + pers.Email.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.phone != null && pers.phone.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CellPhone = '" + pers.phone.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.Password != null && pers.Password.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @LoginPassword = '" + pers.Password.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.DobDay != null && pers.DobDay.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @DobDay = '" + pers.DobDay + "'";
                    paramCnt++;
                }

                if (pers.DobMonth != null && pers.DobMonth.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @DobMonth = '" + pers.DobMonth + "'";
                    paramCnt++;
                }

                if (pers.DobYear != null && pers.DobYear.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @DobYear = '" + pers.DobYear + "'";
                    paramCnt++;
                }

                if (pers.address != null && pers.address.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrAddress = '" + pers.address.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.city != null && pers.city.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrCity = '" + pers.city.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.state != null && pers.state.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrStateId = '" + pers.state + "'";
                    paramCnt++;
                }

                if (pers.country != null && pers.country.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrCountryId = '" + pers.country + "'";
                    paramCnt++;
                }

                if (pers.country != null && pers.country.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ActivationCode = " + activationCode;
                    paramCnt++;
                }

                if (pers.platform != null && pers.platform.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Platform = '" + pers.platform + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                rdr.DbDataReader.Read();

                string isNewUser = rdr.DbDataReader["IsNewUser"].ToString();

                if (isNewUser == "0")
                {
                    updateResp.success = false;
                    updateResp.value = "This email is already used by another account! Use forgot password feature to reset your password.";
                }
                else
                {
                    updateResp.success = true;
                    updateResp.value = rdr.DbDataReader["PersonId"].ToString();
                }

                rdr.DbDataReader.Dispose();


            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.AddPerson'");
            }

            return updateResp;
        }


        public async Task<RegisterResponse> RegisterUser(RegisteredUser registeredUser, string baseUrl)
        {
            RegisterResponse regResp = new RegisterResponse();

            try
            {

                // 1. Generate Activation Code
                string activationCode = _notifRepository.GetActivationCode();

                // 2. Create User record in DB, set as Inactive User, associate activation code

                UpdateResponse brCreateUser = this.AddPerson(baseUrl, registeredUser, activationCode);

                regResp.IsSuccess = brCreateUser.success;
                regResp.Message = brCreateUser.value;

                if (regResp.IsSuccess)
                {


                    // 3. Authenticate user and generate token where Role = Inactive User
                    AuthResponse authResp = _authRepository.GetToken(registeredUser.Email, registeredUser.Password);

                    regResp.user = authResp;
                    regResp.IsSuccess = authResp.IsAuth;
                    regResp.Message = authResp.Message;


                    if (regResp.IsSuccess)
                    {


                        // 4. Send Account Activation Email with Activation Code
                        BasicReponse br = await _notifRepository.SendActivationEmail(registeredUser.Email, activationCode);
                    }
                }
                // Else: Email already exists!
            }
            catch(Exception ex)
            {
                regResp.IsSuccess = false;
                regResp.Message = ex.Message;
            }

            return regResp;
        }

        public async Task<RegisterResponse> UpdateUser(string personId, RegisteredUser registeredUser, string baseUrl)
        {
            RegisterResponse updateResp = new RegisterResponse();

            try
            {

                // 1. Update User record in DB

                BasicReponse br = this.UpdatePerson(baseUrl, registeredUser, personId);

                if (br.IsSuccess)
                {
                    registeredUser.Password = br.Message;

                    // 3. Authenticate user and generate token where Role = Inactive User
                    AuthResponse authResp = _authRepository.GetToken(registeredUser.Email, registeredUser.Password);

                    updateResp.user = authResp;
                    updateResp.IsSuccess = authResp.IsAuth;
                    updateResp.Message = authResp.Message;

                }
                else
                {
                    updateResp.IsSuccess = false;
                    updateResp.Message = br.Message;
                }
            }
            catch (Exception ex)
            {
                updateResp.IsSuccess = false;
                updateResp.Message = ex.Message;
            }

            return updateResp;
        }


        public async Task<BasicReponse> ChangePassword(PersonPasswordChange pc, string personId, string baseUrl)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            BasicReponse br = new BasicReponse();

            try
            {


                if (pc == null)
                {
                    throw new ArgumentNullException(nameof(pc));
                }

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec  PERS.SP_Person_ChangePassword";

                if (personId != null && personId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonID = '" + personId + "'";
                    paramCnt++;
                }

                if (pc.OldPassword != null && pc.OldPassword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @OldPassword = '" + pc.OldPassword.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pc.NewPassword != null && pc.NewPassword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @NewPassword = '" + pc.NewPassword.Replace("'", "''") + "'";
                    paramCnt++;
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() == "1")
                    {
                        br.IsSuccess = true;
                    }
                    else
                    {
                        br.IsSuccess = false;
                        br.Message = "Incorrect Old Password!";
                    }
                }
                else
                {
                    br.IsSuccess = false;
                    br.Message = "Database Error!"; 
                }



                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.ChangePassword'");
            }

            return br;
        }



        public async Task<UserInfoResponse> GetUserDetails(string personId, string baseUrl)
        {
            UserInfoResponse uir = new UserInfoResponse();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_UserInfo_Get";

                if (personId != null && personId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonId = '" + personId + "'";
                    paramCnt++;
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    uir.user.FirstName = rdr.DbDataReader["FirstName"].ToString();
                    uir.user.MiddleName = rdr.DbDataReader["MiddleName"].ToString();
                    uir.user.LastName = rdr.DbDataReader["LastName"].ToString();
                    uir.user.Email = rdr.DbDataReader["Email"].ToString();
                    uir.user.phone = rdr.DbDataReader["CellPhone"].ToString();
                    uir.user.Gender = rdr.DbDataReader["Sex"].ToString();
                    uir.user.DobDay = rdr.DbDataReader["DOBDay"].ToString();
                    uir.user.DobMonth = rdr.DbDataReader["DOBMonth"].ToString();
                    uir.user.DobYear = rdr.DbDataReader["DOBYear"].ToString();

                    uir.user.city = rdr.DbDataReader["CurrCity"].ToString();
                    uir.user.country = rdr.DbDataReader["CurrCountryId"].ToString();
                    uir.user.state = rdr.DbDataReader["CurrStateId"].ToString();
                    uir.user.address = rdr.DbDataReader["CurrAddress"].ToString();
                    uir.user.DOD = rdr.DbDataReader["DOD"].ToString().Split(" ")[0];
                    uir.user.Bio = rdr.DbDataReader["Bio"].ToString();
                }
                else
                {
                    throw new Exception("No records found in database for this Person!");
                }

                rdr.DbDataReader.Dispose();

                uir.IsSuccess = true;


            }
            catch (Exception ex)
            {
                uir.IsSuccess = false;
                uir.Message = ex.Message;
            }

            return uir;
        }

        public async Task<PeopleResponse> GetPeople(string personId, int isActive, int isSaint, string searchKeyword, string baseURL)
        {
            PeopleResponse pr = new PeopleResponse();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_People_Get";

                if (paramCnt > 0)
                    sqlQuery += ", ";
                sqlQuery += " @IsActive = " + isActive;
                paramCnt++;
                

                if (searchKeyword != null && searchKeyword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @SearchKeyword = '" + searchKeyword + "'";
                    paramCnt++;
                }

                if (paramCnt > 0)
                    sqlQuery += ", ";
                sqlQuery += " @IsSaint = " + isSaint;
                paramCnt++;


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    while (rdr.DbDataReader.Read())
                    {
                        PeopleListItem newPeopleListItem = new PeopleListItem();

                        newPeopleListItem.PersonId = rdr.DbDataReader["PersonId"].ToString();
                        newPeopleListItem.FirstName = rdr.DbDataReader["FirstName"].ToString();
                        newPeopleListItem.MiddleName = rdr.DbDataReader["MiddleName"].ToString();
                        newPeopleListItem.LastName = rdr.DbDataReader["LastName"].ToString();
                        newPeopleListItem.DisplayName = newPeopleListItem.FirstName + " " + newPeopleListItem.LastName;
                        newPeopleListItem.HasPic = System.Convert.ToInt32(rdr.DbDataReader["HasPic"].ToString());
                        newPeopleListItem.PictureName = rdr.DbDataReader["PictureName"].ToString();
                        newPeopleListItem.Bio = rdr.DbDataReader["Bio"].ToString();
                        newPeopleListItem.DOD = rdr.DbDataReader["DOD"].ToString();

                        pr.PeopleList.Add(newPeopleListItem);
                    }
                }
                else
                {
                    throw new Exception("No records found in database for this Person!");
                }

                rdr.DbDataReader.Dispose();

                pr.IsSuccess = true;


            }
            catch (Exception ex)
            {
                pr.IsSuccess = false;
                pr.Message = ex.Message;
            }

            return pr;
        }

        public async Task<BasicReponse> SendAccountActivationCode(string email, string baseUrl)
        {
            BasicReponse br = new BasicReponse();

            try
            {

                // 1. Generate Activation Code
                string activationCode = _notifRepository.GetActivationCode();

                // 2. Update Activation Code in DB - PERSON_SECRETS
                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_ActivationCode_Set";

                if (email != null && email.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Email = '" + email + "'";
                    paramCnt++;
                }

                if (activationCode != null && activationCode.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ActivationCode = '" + activationCode + "'";
                    paramCnt++;
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() != "1")
                        throw new Exception("Stored Procedure has failed due to a DB error!");
                }

                rdr.DbDataReader.Dispose();

                // 3. Send Account Activation Email with Activation Code
                br = await _notifRepository.SendActivationEmail(email, activationCode);

            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }

            return br;
        }

        public async Task<BasicReponse> ActivateAccount(string email, string activationCode, string baseUrl)
        {
            BasicReponse br = new BasicReponse();

            try
            {
                // 1. Set PERSON as Active if activation Code is valid
                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_ActivateAccount_Set";

                if (email != null && email.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Email = '" + email + "'";
                    paramCnt++;
                }

                if (activationCode != null && activationCode.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ActivationCode = '" + activationCode + "'";
                    paramCnt++;
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() != "1")
                        throw new Exception("Invalid Activation Code! Enter the latest activation code that you received or resend a new code.");
                }

                rdr.DbDataReader.Dispose();

                br.IsSuccess = true;

            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }

            return br;
        }

        public async Task<BasicReponse> DeleteAccount(string email, string baseUrl)
        {
            BasicReponse br = new BasicReponse();

            try
            {
                // 1. Set PERSON as Active if activation Code is valid
                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_Person_Inactive_Delete";

                if (email != null && email.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Email = '" + email + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() != "1")
                        throw new Exception("Error has occurred during deletion of inactive user!");
                }

                rdr.DbDataReader.Dispose();

                br.IsSuccess = true;

            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }

            return br;
        }

        public async Task<AuthResponse> SetHasPic(int hasPic, string personId, string baseUrl, TokenPayLoad tokenPayload, string fileName)
        {
            AuthResponse ar = new AuthResponse();

            try
            {
                // 1. Set PERSON HasPic
                string sqlQuery;

                sqlQuery = @"exec PERS.SP_Person_HasPic_Set @PersonId = '" + personId + "', @HasPic = '" + hasPic + "', @FileName = '" + fileName + "'";

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                rdr.DbDataReader.Dispose();

                tokenPayload.PicFileName = fileName;
                tokenPayload.HasPic = true;

                ar = _authRepository.RenewToken(tokenPayload);

            }
            catch (Exception ex)
            {
                ar.IsAuth = false;
                ar.Message = ex.Message;
            }

            return ar;
        }

        public async Task<BasicReponse> VerifyPhone(string phone, string baseUrl)
        {
            BasicReponse br = new BasicReponse();

            try
            {

                // 1. Generate Activation Code
                string activationCode = _notifRepository.GetActivationCode();

                // 2. Send Phone Verification Code as SMS
                br = await _notifRepository.SendActivationSMS(phone, activationCode);
                
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }

            return br;
        }

        public async Task<RegisterResponse> LoginUser(AuthRequest authRequest, string baseUrl)
        {
            RegisterResponse regResp = new RegisterResponse();

            try
            {
                // 1. Authenticate user and generate token where Role = Inactive User
                AuthResponse authResp = _authRepository.GetToken(authRequest.UserId, authRequest.Password);

                regResp.user = authResp;
                regResp.IsSuccess = authResp.IsAuth;
                regResp.Message = authResp.Message;
            }
            catch (Exception ex)
            {
                regResp.IsSuccess = false;
                regResp.Message = ex.Message;
            }

            return regResp;
        }

        public async Task<BasicReponse> ResetPassword(string email, string baseUrl)
        {
            BasicReponse br = new BasicReponse();

            try
            {
                // 1. Generate temp password
                string tempPassword = _notifRepository.GetTempPassword();

                // 2. Persist Temp Password in Person Secrets
                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_TempPassword_Set";

                if (email != null && email.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Email = '" + email + "'";
                    paramCnt++;
                }

                if (tempPassword != null && tempPassword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @TempPassword = '" + tempPassword.Replace("'", "''") + "'";
                    paramCnt++;
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() != "1")
                        throw new Exception("Reset Password has failed due to a DB error!");
                }

                rdr.DbDataReader.Dispose();

                // 3. Send Email with Temp Password

                br = await _notifRepository.SendTempPasswordEmail(email, tempPassword);
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }
            return br;
        }


        #region Not used
        public PersonResponse GetPerson(string baseURL, string token, string personID, string churchID, string MyFriends, string ChurchRoleID, string RequestorID,
            string isAddFriend, string isAddPriest, string isAddDeacon, string isAddChurchMember, string AddChurchID, string isAddClassMember, string AddClassID, string ClassRole)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonResponse PersonResp = new PersonResponse();
            var lstPerson = new List<Person>();

            try
            {
                string sqlQuery;

                sqlQuery = @"exec PERS.[SP_Person_Get]";
                int paramCnt = 0;

                if (personID != null && personID.Trim() != "")
                {
                    sqlQuery += " @PersonID = '" + personID + "'";
                    paramCnt++;
                }

                if (churchID != null && churchID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChurchID = '" + churchID + "'";
                    paramCnt++;
                }

                if (MyFriends != null && MyFriends.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @MyFriends = '" + MyFriends + "'";
                    paramCnt++;
                }
                if (ChurchRoleID != null && ChurchRoleID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChurchRoleID = '" + ChurchRoleID + "'";
                    paramCnt++;
                }

                if (RequestorID != null && RequestorID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @RequestorID = '" + RequestorID + "'";
                    paramCnt++;
                }

                if (isAddFriend != null && isAddFriend.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @isAddFriend = '" + isAddFriend + "'";
                    paramCnt++;
                }

                if (isAddPriest != null && isAddPriest.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @isAddPriest = '" + isAddPriest + "'";
                    paramCnt++;
                }

                if (isAddDeacon != null && isAddDeacon.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @isAddDeacon = '" + isAddDeacon + "'";
                    paramCnt++;
                }

                if (isAddChurchMember != null && isAddChurchMember.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @isAddChurchMember = '" + isAddChurchMember + "'";
                    paramCnt++;
                }

                if (AddChurchID != null && AddChurchID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @AddChurchID = '" + AddChurchID + "'";
                    paramCnt++;
                }

                if (isAddClassMember != null && isAddClassMember.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @isAddClassMember = '" + isAddClassMember + "'";
                    paramCnt++;
                }

                if (AddClassID != null && AddClassID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @AddClassID = '" + AddClassID + "'";
                    paramCnt++;
                }

                if (ClassRole != null && ClassRole.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ClassRole = '" + ClassRole + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    Person pr = new Person();

                    pr.PersonID = rdr.DbDataReader["PersonID"].ToString();
                    //pr.PersonName = rdr.DbDataReader["PersonName"].ToString();
                    pr.FirstName = rdr.DbDataReader["FirstName"].ToString();
                    pr.MiddleName = rdr.DbDataReader["MiddleName"].ToString();
                    pr.LastName = rdr.DbDataReader["LastName"].ToString();
                    pr.CellPhone = rdr.DbDataReader["CellPhone"].ToString();
                    pr.Email = rdr.DbDataReader["Email"].ToString();
                    pr.DOB = rdr.DbDataReader["DOB"].ToString();

                    pr.DOBYear = rdr.DbDataReader["DOBYear"].ToString();
                    pr.DOBMonth = rdr.DbDataReader["DOBMonth"].ToString();
                    pr.DOBDay = rdr.DbDataReader["DOBDay"].ToString();

                    //pr.ClassID = rdr.DbDataReader["ClassID"].ToString();
                    //pr.ClassName = rdr.DbDataReader["ClassName"].ToString();
                    //pr.AttendPerc = rdr.DbDataReader["AttendPerc"].ToString();
                    //pr.AttendCount = rdr.DbDataReader["AttendCount"].ToString();
                    //pr.LessonsCount = rdr.DbDataReader["LessonsCount"].ToString();
                    //pr.YearsOld = rdr.DbDataReader["YearsOld"].ToString();

                    pr.Sex = rdr.DbDataReader["Sex"].ToString();
                    //pr.MartialStatusID = rdr.DbDataReader["MartialStatusID"].ToString();
                    //pr.MartialStatusName = rdr.DbDataReader["MartialStatusName"].ToString();
                    //pr.BirthCountryID = rdr.DbDataReader["BirthCountryID"].ToString();
                    //pr.BirthCountryName = rdr.DbDataReader["BirthCountryName"].ToString();
                    //pr.BirthStateID = rdr.DbDataReader["BirthStateID"].ToString();
                    //pr.BirthCityID = rdr.DbDataReader["BirthCityID"].ToString();
                    //pr.BirthCityName = rdr.DbDataReader["BirthCityName"].ToString();
                    //pr.BirthZipCode = rdr.DbDataReader["BirthZipCode"].ToString();
                    //pr.CurrAddressID = rdr.DbDataReader["CurrAddressID"].ToString();
                    //pr.CurrPrimChurchID = rdr.DbDataReader["CurrPrimChurchID"].ToString();
                    //pr.AptNum = rdr.DbDataReader["AptNum"].ToString();
                    //pr.CityID = rdr.DbDataReader["CityID"].ToString();
                    //pr.CityName = rdr.DbDataReader["CityName"].ToString();
                    //pr.CountryID = rdr.DbDataReader["CountryID"].ToString();
                    //pr.CountryName = rdr.DbDataReader["CountryName"].ToString();
                    //pr.StateID = rdr.DbDataReader["StateID"].ToString();
                    //pr.StreetName = rdr.DbDataReader["StreetName"].ToString();
                    //pr.StreetNum = rdr.DbDataReader["StreetNum"].ToString();
                    //pr.ZipCode = rdr.DbDataReader["ZipCode"].ToString();
                    //pr.LoginPassword = rdr.DbDataReader["LoginPassword"].ToString();
                    //pr.CurrPrimChurchName = rdr.DbDataReader["CurrPrimChurchName"].ToString();
                    pr.IsActive = rdr.DbDataReader["IsActive"].ToString();
                    //pr.HomePhone = rdr.DbDataReader["HomePhone"].ToString();
                    //pr.Industry = rdr.DbDataReader["Industry"].ToString();
                    //pr.Occupation = rdr.DbDataReader["Occupation"].ToString();
                    //pr.FatherOfConfession = rdr.DbDataReader["FatherOfConfession"].ToString();

                    //pr.Relation = rdr.DbDataReader["Relation"].ToString();
                    //pr.StatusID = rdr.DbDataReader["StatusID"].ToString();
                    //pr.StatusName = rdr.DbDataReader["StatusName"].ToString();
                    //pr.Bio = rdr.DbDataReader["Bio"].ToString();
                    //pr.DOD = rdr.DbDataReader["DOD"].ToString();
                    //pr.IsSaint = rdr.DbDataReader["IsSaint"].ToString();
                    //pr.HasPic = rdr.DbDataReader["HasPic"].ToString();
                    //pr.PictureName = rdr.DbDataReader["PictureName"].ToString();
                    lstPerson.Add(pr);
                }
                rdr.DbDataReader.Dispose();
            }

            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.GetPerson'");
            }
            PersonResp.PersonList = lstPerson;
            PersonResp.PersonListCount = lstPerson.Count;
            return PersonResp;
        }

        public PersonResponse GetBirthdaysOfWeek(string baseURL, string token, string churchID, string classID, string nextSunday, string MonthOrWeek, string Month)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonResponse PersonResp = new PersonResponse();
            var lstPerson = new List<Person>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec PERS.[SP_BirthdaysOfWeek_Get]";
                int paramCnt = 0;

                if (churchID != null && churchID.Trim() != "")
                {
                    sqlQuery += " @ChurchID = '" + churchID + "'";
                    paramCnt++;
                }

                if (classID != null && classID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ClassID = '" + classID + "'";
                    paramCnt++;
                }

                if (nextSunday != null && nextSunday.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @NextSunday = '" + nextSunday + "'";
                    paramCnt++;
                }

                if (MonthOrWeek != null && MonthOrWeek.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @MonthOrWeek = '" + MonthOrWeek + "'";
                    paramCnt++;
                }

                if (Month != null && Month.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Month = '" + Month + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    Person pr = new Person();

                    pr.PersonID = rdr.DbDataReader["PersonID"].ToString();
                    //pr.PersonName = rdr.DbDataReader["PersonName"].ToString();
                    pr.CellPhone = rdr.DbDataReader["CellPhone"].ToString();
                    pr.Email = rdr.DbDataReader["Email"].ToString();

                    pr.DOB = rdr.DbDataReader["DOB"].ToString();
                    pr.DOBYear = rdr.DbDataReader["DOBYear"].ToString();
                    pr.DOBMonth = rdr.DbDataReader["DOBMonth"].ToString();
                    pr.DOBDay = rdr.DbDataReader["DOBDay"].ToString();

                    //pr.ClassID = rdr.DbDataReader["ClassID"].ToString();
                    //pr.YearsOld = rdr.DbDataReader["YearsOld"].ToString();
                    //pr.PictureName = rdr.DbDataReader["PictureName"].ToString();

                    lstPerson.Add(pr);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.GetBirthdaysOfWeek'");
            }
            PersonResp.PersonList = lstPerson;
            PersonResp.PersonListCount = lstPerson.Count;
            return PersonResp;
        }

        public PersonResponse PersonDeActivate(string baseURL, string token, string personID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonResponse personResp = new PersonResponse();
            var lstPerson = new List<Person>();

            try
            {



                string sqlQuery;

                sqlQuery = @"exec PERS.[SP_Person_DeActivate]";
                int paramCnt = 0;

                if (personID != null && personID.Trim() != "")
                {
                    sqlQuery += " @personID = '" + personID + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.PersonDeActivate'");
            }
            personResp.PersonList = lstPerson;
            personResp.PersonListCount = lstPerson.Count;
            return personResp;
        }

        public PersonResponse PersonActivate(string baseURL, string token, string personID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonResponse personResp = new PersonResponse();
            var lstPerson = new List<Person>();

            try
            {
                string sqlQuery;

                sqlQuery = @"exec PERS.[SP_Person_Activate]";
                int paramCnt = 0;

                if (personID != null && personID.Trim() != "")
                {
                    sqlQuery += " @personID = '" + personID + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.PersonActivate'");
            }
            personResp.PersonList = lstPerson;
            personResp.PersonListCount = lstPerson.Count;
            return personResp;
        }


        public PersonLookupsResponse GetLookups(string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonLookupsResponse personLookupsResp = new PersonLookupsResponse();
            var lstLocCountry = new List<LocCountry>();
            var lstLocState = new List<LocState>();
            var lstLocCity = new List<LocCity>();
            var lstMartialStatus = new List<MartialStatus>();
            var lstChurches = new List<Church>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec PERS.[SP_Person_GetLookups]";


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    LocCountry locCountry = new LocCountry();

                    locCountry.CountryID = rdr.DbDataReader["CountryID"].ToString();
                    locCountry.CountryName = rdr.DbDataReader["CountryName"].ToString();
                    lstLocCountry.Add(locCountry);
                }
                rdr.DbDataReader.NextResult();

                while (rdr.DbDataReader.Read())
                {
                    LocState locState = new LocState();

                    locState.StateID = rdr.DbDataReader["StateID"].ToString();
                    locState.StateName = rdr.DbDataReader["StateName"].ToString();
                    locState.CountryID = rdr.DbDataReader["CountryID"].ToString();
                    lstLocState.Add(locState);
                }

                rdr.DbDataReader.NextResult();

                while (rdr.DbDataReader.Read())
                {
                    LocCity locCity = new LocCity();

                    locCity.CityID = System.Convert.ToInt32(rdr.DbDataReader["CityID"]);
                    locCity.CityName = rdr.DbDataReader["CityName"].ToString();
                    locCity.CountryID = rdr.DbDataReader["CountryID"].ToString();
                    locCity.StateID = rdr.DbDataReader["StateID"].ToString();
                    lstLocCity.Add(locCity);
                }

                rdr.DbDataReader.NextResult();

                while (rdr.DbDataReader.Read())
                {
                    MartialStatus martialStatus = new MartialStatus();

                    martialStatus.MartialStatusID = rdr.DbDataReader["MartialStatusID"].ToString();
                    martialStatus.MartialStatusName = rdr.DbDataReader["MartialStatusName"].ToString();
                    lstMartialStatus.Add(martialStatus);
                }

                rdr.DbDataReader.NextResult();

                while (rdr.DbDataReader.Read())
                {
                    Church ch = new Church();

                    ch.ChurchID = rdr.DbDataReader["ChurchID"].ToString();
                    ch.ChurchName = rdr.DbDataReader["ChurchName"].ToString();
                    lstChurches.Add(ch);
                }

                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.GetLookups'");
            }
            personLookupsResp.LocCountryList = lstLocCountry;
            personLookupsResp.LocCountryListCount = lstLocCountry.Count;

            personLookupsResp.LocStateList = lstLocState;
            personLookupsResp.LocStateListCount = lstLocState.Count;

            personLookupsResp.LocCityList = lstLocCity;
            personLookupsResp.LocCityListCount = lstLocCity.Count;

            personLookupsResp.MartialStatusList = lstMartialStatus;
            personLookupsResp.MartialStatusListCount = lstMartialStatus.Count;

            personLookupsResp.ChurchesList = lstChurches;
            personLookupsResp.ChurchesListCount = lstChurches.Count;

            return personLookupsResp;
        }



        public UpdateResponse AddPersonRelation(PersonRelation pr)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {

                if (pr == null)
                {
                    throw new ArgumentNullException(nameof(pr));
                }

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_Person_Relations_Add";

                if (pr.Person1ID != null && pr.Person1ID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Person1ID = '" + pr.Person1ID + "'";
                    paramCnt++;
                }

                if (pr.Person2ID != null && pr.Person2ID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Person2ID = '" + pr.Person2ID + "'";
                    paramCnt++;
                }

                if (pr.Relation != null && pr.Relation.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Relation = '" + pr.Relation + "'";
                    paramCnt++;
                }

                if (pr.StatusID != null && pr.StatusID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @StatusID = '" + pr.StatusID + "'";
                    paramCnt++;
                }

                if (pr.RelationRequestor != null && pr.RelationRequestor.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @RelationRequestor = '" + pr.RelationRequestor + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                updateResp.success = true;

                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.AddPersonRelation'");
            }

            return updateResp;
        }

        public UpdateResponse UpdatePersonRelation(PersonRelation pr)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {

                if (pr == null)
                {
                    throw new ArgumentNullException(nameof(pr));
                }

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_Person_Relations_Update";

                if (pr.Person1ID != null && pr.Person1ID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Person1ID = '" + pr.Person1ID + "'";
                    paramCnt++;
                }

                if (pr.Person2ID != null && pr.Person2ID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Person2ID = '" + pr.Person2ID + "'";
                    paramCnt++;
                }

                if (pr.Relation != null && pr.Relation.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Relation = '" + pr.Relation + "'";
                    paramCnt++;
                }

                if (pr.StatusID != null && pr.StatusID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @StatusID = '" + pr.StatusID + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                updateResp.success = true;

                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.UpdatePersonRelation'");
            }

            return updateResp;
        }

        public PersonRelationsResponse GetPersonRelations(string baseURL, string token, string PersonID, string Relation)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonRelationsResponse personRelationsResp = new PersonRelationsResponse();
            var lstPersonRelations = new List<PersonRelation>();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_Person_Relations_Get";

                if (PersonID != null && PersonID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonID = '" + PersonID + "'";
                    paramCnt++;
                }

                if (Relation != null && Relation.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Relation = '" + Relation + "'";
                    paramCnt++;
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    PersonRelation pr = new PersonRelation();

                    //Read fields from DataReader
                    //-----------------------------
                    pr.Person1ID = rdr.DbDataReader["PersonID"].ToString();
                    pr.Person1Name = rdr.DbDataReader["PersonName"].ToString();
                    pr.Person2ID = rdr.DbDataReader["RelID"].ToString();
                    pr.Person2Name = rdr.DbDataReader["RelName"].ToString();
                    pr.Relation = rdr.DbDataReader["Relation"].ToString();
                    pr.StatusID = rdr.DbDataReader["StatusID"].ToString();
                    pr.LastModified = rdr.DbDataReader["LastModified"].ToString();
                    pr.CreatedDT = rdr.DbDataReader["CreatedDT"].ToString();
                    pr.RelationAs = rdr.DbDataReader["RelationAs"].ToString();

                    pr.RelationRequestor = rdr.DbDataReader["RelationRequestor"].ToString();


                    lstPersonRelations.Add(pr);
                }
                rdr.DbDataReader.Dispose();
            }

            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.GetPersonRelations'");
            }
            personRelationsResp.PersonRelationsList = lstPersonRelations;
            personRelationsResp.PersonRelationsListCount = lstPersonRelations.Count;
            return personRelationsResp;
        }

        public RelationsResponse GetRelationLookup(string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            RelationsResponse relationsResp = new RelationsResponse();
            var lstRelations = new List<Relation>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec PERS.SP_Relation_GetLookups";


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    Relation r = new Relation();

                    //Read fields from DataReader
                    //-----------------------------
                    r.RelationID = rdr.DbDataReader["RelationID"].ToString();
                    r.RelationName = rdr.DbDataReader["RelationName"].ToString();


                    lstRelations.Add(r);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.GetRelations'");
            }
            relationsResp.RelationsList = lstRelations;
            relationsResp.RelationsListCount = lstRelations.Count;
            return relationsResp;
        }

        




        public BasicReponse UpdatePerson(string baseURL, RegisteredUser pers, string personId)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            BasicReponse updateResp = new BasicReponse();

            try
            {
                string sqlQuery;

                int paramCnt = 0;

                sqlQuery = @"exec PERS.[SP_Person_Update]";

                if (personId != null && personId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonID = '" + personId + "'";
                    paramCnt++;
                }

                if (pers.FirstName != null && pers.FirstName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @FirstName = '" + pers.FirstName.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.LastName != null && pers.LastName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @LastName = '" + pers.LastName.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.MiddleName != null && pers.MiddleName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @MiddleName = '" + pers.MiddleName.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.Gender != null && pers.Gender.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Sex = '" + pers.Gender + "'";
                    paramCnt++;
                }


                if (pers.phone != null && pers.phone.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CellPhone = '" + pers.phone.Replace("'", "''") + "'";
                    paramCnt++;
                }


                if (pers.DobDay != null && pers.DobDay.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @DobDay = '" + pers.DobDay + "'";
                    paramCnt++;
                }

                if (pers.DobMonth != null && pers.DobMonth.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @DobMonth = '" + pers.DobMonth + "'";
                    paramCnt++;
                }

                if (pers.DobYear != null && pers.DobYear.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @DobYear = '" + pers.DobYear + "'";
                    paramCnt++;
                }

                if (pers.country != null && pers.country.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrCountryId = '" + pers.country + "'";
                    paramCnt++;
                }

                if (pers.state != null && pers.state.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrStateId = '" + pers.state + "'";
                    paramCnt++;
                }

                if (pers.city != null && pers.city.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrCity = '" + pers.city.Replace("'", "''") + "'";
                    paramCnt++;
                }

                if (pers.address != null && pers.address.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @CurrAddress = '" + pers.address.Replace("'", "''") + "'";
                    paramCnt++;
                }
                

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();
                    if (rdr.DbDataReader["IsSuccess"].ToString() == "1")
                    {
                        updateResp.IsSuccess = true;
                        updateResp.Message = rdr.DbDataReader["MyPass"].ToString(); // Contains user password to regenerate auth token
                    }
                }
                else
                {
                    updateResp.IsSuccess = false;
                    updateResp.Message = "Database error has occured!";
                }
                

                rdr.DbDataReader.Dispose();
            }


            catch (Exception ex)
            {
                updateResp.IsSuccess = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.UpdatePerson'");
            }

            return updateResp;
        }
        #endregion

    }
}
