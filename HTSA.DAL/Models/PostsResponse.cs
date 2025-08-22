using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PostsResponse
    {
        //public AuthResponse authResponse { get; set; }
        public string parentURL { get; set; }
        public int PostsListCount { get; set; }
        public List<Post> PostsList { get; set; }

        

    }


}
