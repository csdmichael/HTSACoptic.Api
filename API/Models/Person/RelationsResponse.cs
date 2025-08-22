using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class RelationsResponse
    {
        public string parentURL { get; set; }
        public int RelationsListCount { get; set; }
        public List<Relation> RelationsList { get; set; }
        

    }


}
