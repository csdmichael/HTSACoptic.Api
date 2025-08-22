//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class PersonRelation
    {
        public string Token { get; set; }
        public string Person1ID { get; set; }
        public string Person1Name { get; set; }
        public string Person2ID { get; set; }
        public string Person2Name { get; set; }
        public string Relation { get; set; }
	    public string StatusID { get; set; }
	    public string LastModified { get; set; }
        public string CreatedDT { get; set; }
        public string RelationAs { get; set; }
        public string RelationRequestor { get; set; }


}
}