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
        public string title { get; set; }
        public string post { get; set; }

        public string content { get; set; }

        public string created { get; set; }
        public int likes { get; set; }
        public Post()
        {
            likes = 0;
            content = string.Empty;
        }

    }
}