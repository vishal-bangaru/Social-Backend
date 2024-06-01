using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
namespace Common.Models
{
    public class Post
    {
        public string id { get; set; }      
        public string user_id { get; set; }
        public string type {  get; set; }
        public string title { get; set; }
        public string owner {  get; set; }
        public string post { get; set; }

        public string content { get; set; }

        public DateTime created { get; set; }
        public List<string> likes { get; set; }
        public List<Comment> comments { get; set; }
        public Post(string userId)
        {
            id = Guid.NewGuid().ToString();
            user_id = userId;
            type = "Post";
            content = string.Empty;
            created = DateTime.Now;
            comments = new List<Comment>();
            likes = new List<string>();
        }

    }
}