using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Likes
    {
        public string user_id {  get; set; }
        public Likes() { 
           user_id = string.Empty;
        }
    }
    
}
