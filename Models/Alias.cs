using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class Alias
    {
        public string AliasName { get; set; }
        public User Assignee { get; }
        public User Assignor { get; }
        public Alias (string aliasName, User assignee, User assignor)
        {
            AliasName = aliasName;
            Assignee = assignee;
            Assignor = assignor;
        }
    }
}
