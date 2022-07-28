using ChatServerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Guid FolderId { get; set; }
        public FileType Type { get; set; }
        public DateTime Created { get; set; }
        public File(String name, Guid folderId, FileType fileType)
        {
            Id = Guid.NewGuid();
            Name = name;
            FolderId = folderId;
            Type = fileType;
            Created = DateTime.Now;
        }
    }
}
