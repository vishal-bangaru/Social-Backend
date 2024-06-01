using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Comment
    {
        public string name { get; set; } 
        public string post_id { get; set; }
        public string desc { get; set;}
        public Comment() { 
        name = string.Empty;
        post_id = string.Empty;
        desc = string.Empty;
        }    
        
    }
}
