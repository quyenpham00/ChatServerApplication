using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class PrivateGroup : Group
    {
        public List<User> Admins { get; set; }
        public PrivateGroup(String name, User admin) : base(name)
        {
            Admins = new List<User>();
            Admins.Add(admin);
        }
    }
}
