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
        public Guid Id { get; }
        public Guid SenderID { get; }
        public Guid ReceiverID { get; }
        public Receiver ReceiverType { get; set; }
        public List<Attachment> Attachments { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }

        public Message(Guid senderID, Guid receiverID)
        {
            Id = Guid.NewGuid();
            SenderID = senderID;
            ReceiverID = receiverID;
            Created = DateTime.Now;
        }

        public bool IsRelatedToGroup(Guid senderID, Guid receiverID)
        {
            return SenderID == senderID || ReceiverID == receiverID && ReceiverType == Receiver.Group;
        }

        public bool IsRelatedToUser(Guid senderID, Guid receiverID)
        {
            return SenderID == senderID || ReceiverID == receiverID && ReceiverType == Receiver.User;
        }
    }
}
