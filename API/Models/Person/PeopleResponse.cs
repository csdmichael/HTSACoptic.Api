
using HTSA.DAL.Models;
using System.Collections.Generic;

namespace HTSA.API.Models
{
    public class PeopleResponse: BasicReponse
    {
        public List<PeopleListItem> PeopleList { get; set; }

        public PeopleResponse()
        {
            PeopleList = new List<PeopleListItem>();
        }
    }
}
