using ChatServerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderID { get; set; }
        public Guid GroupID { get; set; }
        public List<File> Files { get; set; }
        public String Content { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime Created { get; set; }

        public Message (Guid senderID, Guid groupID, String content, List<File> files = null)
        {
            Id = Guid.NewGuid();
            SenderID = senderID;
            GroupID = groupID;
            Files = files;
            Content = content;
            Status = MessageStatus.Sent;
            Created = DateTime.Now;
        }
    }
}
