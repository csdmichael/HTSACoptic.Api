using Microsoft.Extensions.Logging;
using HTSA.Models;
using System;
using HTSA.API.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using HTSA.DAL.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class NotifRepository : INotifRepository
    {
        ILogger _logger;
        ApplContext _context;
        IConfigurationRoot _configuration { get; }
        IConfigurationSection notifSettings;

        public NotifRepository(ApplContext context, ILogger<NotifRepository> logger, IConfigurationRoot configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            notifSettings = _configuration.GetSection("Notif");
        }

        public string GetActivationCode()
        {
            Random r = new Random();
            int rInt = r.Next(100000, 999999); //for ints
            return rInt.ToString();
        }

        #region Email

        public async Task<BasicReponse> SendActivationEmail(string toAddress, string activationCode)
        {
            EmailModel emailObj = new EmailModel();

            emailObj.Subject = "Account Activation Code";
            emailObj.From = notifSettings.GetValue<string>("sendGridFromAddress");
            emailObj.To = toAddress;
            emailObj.Body = "Your Account Activation Code is: <b>" + activationCode + "</b>";

            return await SendEmail(emailObj);
        }

        public async Task<BasicReponse> SendActivationSMS(string phone, string activationCode)
        {
            SMSModel smsObj = new SMSModel();

            smsObj.Title = "Phone Verification Code";
            smsObj.phone =phone;
            smsObj.Msg = "Your Phone Verification Code is: " + activationCode;

            return await SendSMS(smsObj);
        }

        public async Task<BasicReponse> SendSMS(SMSModel smsObj)
        {
            EmailModel emailObj = new EmailModel();
            emailObj.Subject = smsObj.Title;
            emailObj.Body = smsObj.Msg;
            emailObj.From = notifSettings.GetValue<string>("textMagicFromAddress");
            emailObj.To = smsObj.phone + "@textmagic.com";
            return await SendEmail(emailObj);
        }

        private bool IsEmailAllowed(string email)
        {
            bool isEmailAllowed = false;

            try
            {



                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PERS.SP_Person_EmailLastSent_Set";

                

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

                    if (rdr.DbDataReader["AllowEmail"].ToString() == "1")
                    {
                        isEmailAllowed = true;
                    }
                    else
                    {
                        isEmailAllowed = false;
                    }
                }
                

                rdr.DbDataReader.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return isEmailAllowed;
        }

        public async Task<BasicReponse> SendEmail(EmailModel emailObj)
        {
            BasicReponse ur = new BasicReponse();

            try
            {
                if (IsEmailAllowed(emailObj.To))
                {
                    var apiKey = notifSettings.GetValue<string>("sendGridApiKey");
                    var client = new SendGridClient(apiKey);
                    if (emailObj.From == null || emailObj.From == "")
                    {
                        emailObj.From = notifSettings.GetValue<string>("sendGridFromAddress");
                    }
                    var from = new EmailAddress(emailObj.From);
                    var to = new EmailAddress(emailObj.To);
                    string htmlContent = emailObj.Body;
                    string subj = "HTSA Coptic App - " + emailObj.Subject;
                    var msg = MailHelper.CreateSingleEmail(from, to, subj, emailObj.Body, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        ur.IsSuccess = true;
                    }
                    else
                    {
                        ur.IsSuccess = false;
                        ur.Message = response.Body.ReadAsStringAsync().Result;
                    }
                }
                else
                {
                    ur.IsSuccess = false;
                    ur.Message = "You have already sent an email. Please, check your inbox, junk and spam folders!";
                }

            }
            catch (Exception ex)
            {
                ur.IsSuccess = false;
                ur.Message = ex.Message;
            }

            return ur;
        }

        public async Task<BasicReponse> SendTempPasswordEmail(string email, string tempPassword)
        {
            EmailModel emailObj = new EmailModel();

            emailObj.Subject = "Temporary Password";
            emailObj.From = notifSettings.GetValue<string>("sendGridFromAddress");
            emailObj.To = email;
            emailObj.Body = "Your Temporary Password is: <b>" + tempPassword + "</b>";

            return await SendEmail(emailObj);
        }

        public string GetTempPassword()
        {
            string tempPass = Guid.NewGuid().ToString();
            tempPass = tempPass.Substring(0, 8);

            return tempPass;
        }
        #endregion

    }
}
