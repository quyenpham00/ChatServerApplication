using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class PublicGroup : Group
    {
        public String AccessCode { get; set; }
        public PublicGroup (String name,string accessCode) : base (name)
        {
            AccessCode = accessCode;
        }
    }
}
