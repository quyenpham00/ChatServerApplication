using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class PrivateGroup : Group
    {
        public User Admin { get; set; }
        public string AccessCode { get; set; }
    }
}
