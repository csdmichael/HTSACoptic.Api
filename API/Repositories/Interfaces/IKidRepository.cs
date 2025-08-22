using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IKidRepository
    {
  
        KidsResponse GetKidsByClass(string baseURL, string token, string classID);

    }
}