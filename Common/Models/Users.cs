using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Users
    {
        public string id { get; set; } 
        public string type { get; set; }    
        public string user_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public List<string> followers { get; set; }
        public List<string> following { get; set; }
        public List<string> requests { get; set; }
        public List<string> approvals { get; set; }


        //public Users()
        //{
        //    //posts = new List<Post>();
        //   type= "Users";   
        //    user_id = Guid.NewGuid().ToString();
        //    id = user_id;
        //    followers = new List<string>();
        //    following = new List<string>();
        //}
    }
}