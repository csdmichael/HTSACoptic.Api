using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonChurchRolesResponse
    {
        public string parentURL { get; set; }
        public int PersonChurchRolesListCount { get; set; }
        public List<PersonChurchRole> PersonChurchRolesList { get; set; }
        

    }


}
