using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonModuleRolesResponse
    {
        public string parentURL { get; set; }
        public int PersonModuleRolesListCount { get; set; }
        public List<PersonModuleRole> PersonModuleRolesList { get; set; }
        

    }


}
