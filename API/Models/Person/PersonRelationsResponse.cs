using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonRelationsResponse
    {
        public string parentURL { get; set; }
        public int PersonRelationsListCount { get; set; }
        public List<PersonRelation> PersonRelationsList { get; set; }
        

    }


}
