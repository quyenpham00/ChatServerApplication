using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<File> Files { get; set; }

        public Folder(String name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Files = new List<File>();
        }
    }
}
