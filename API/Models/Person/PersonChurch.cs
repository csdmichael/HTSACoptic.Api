//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class PersonChurch: Church
    {
        // public string ChurchID { get; set; }
	    public string PersonID { get; set; }
        public string PersonName { get; set; }
        public string Email { get; set; }
        public string JoinTime { get; set; }
	    public string StatusID { get; set; }
	    public string LastModified { get; set; }
	    public string CreatedDT { get; set; }


    }
}