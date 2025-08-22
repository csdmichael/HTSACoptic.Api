
namespace HTSA.API.Models
{
    public class PeopleListItem
    {
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public int HasPic { get; set; }
        public string PictureName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string DOD { get; set; }
    }
}

