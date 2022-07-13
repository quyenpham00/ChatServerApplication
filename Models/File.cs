using ChatServerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    internal class File
    {
       public string Id { get; set; }
       public FileType FileType { get; set; }
    }
}
