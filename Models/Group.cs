using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class Group
    {
        public List<User> Users { get; set; }
        public string Name { get; set; }
    }
}
