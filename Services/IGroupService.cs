using ChatServerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Services
{
    internal interface IGroupService
    {
        void CreateGroup(string groupName, string groupType, User groupAdmin);
        bool JoinGroupChatByAccessCode(Group group, string accessCode, User user);
        bool InviteMemberToGroupChat(Group group, User invitor, User member);
        List<Attachment> FindAllFiles(Guid senderID, Guid receiverID);
    }
}
