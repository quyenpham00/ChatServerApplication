using ChatServerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AttachmentType Type
        {
            get { return this.Type; }
            set
            {
                this.Type = classifyAttachmentType(Name);
            }
        }
        public byte[] Content { get; set; }
        public DateTime Created { get; set; }
        public Attachment(string name, byte[] content)
        {
            Id = Guid.NewGuid();
            Name = name;
            Created = DateTime.Now;
            Content = content;
        }

        private AttachmentType classifyAttachmentType(string fileName)
        {
            string extention = Path.GetExtension(fileName).ToLower();
            switch (extention)
            {
                case "png":
                case "jpeg":
                case "jpg":
                    return AttachmentType.Image;
                case "mtm":
                case "abc":
                case "flp":
                    return AttachmentType.Audio;
                case "mp4":
                case "mov":
                case "webm":
                    return AttachmentType.Video;
                default:
                    return AttachmentType.File;
            }
        }
    }
}
