using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class PublicGroup : Group
    {
        public string AccessCode { get; set; }
        public PublicGroup(string name, string accessCode) : base(name)
        {
            AccessCode = accessCode;
        }
    }
}
