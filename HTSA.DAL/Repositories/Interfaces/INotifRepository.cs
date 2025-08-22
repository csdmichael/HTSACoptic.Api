using HTSA.API.Models;
using HTSA.DAL.Models;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface INotifRepository
    {
        Task<BasicReponse> SendEmail(EmailModel emailObj);
        Task<BasicReponse> SendActivationEmail(string toAddress, string activationCode);
        Task<BasicReponse> SendTempPasswordEmail(string email, string tempPassword);

        string GetActivationCode();
        string GetTempPassword();

        Task<BasicReponse> SendActivationSMS(string phone, string activationCode);
        Task<BasicReponse> SendSMS(SMSModel smsObj);
        // bool IsEmailAllowed(string email);
    }
}