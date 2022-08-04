using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class PrivateGroup : Group
    {
        public List<User> Admins { get; }
        public PrivateGroup(string name, User admin) : base(name)
        {
            Admins = new List<User>();
            Admins.Add(admin);
        }
    }
}
