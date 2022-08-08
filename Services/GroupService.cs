using ChatServerApplication.Data;
using ChatServerApplication.Models;
using ChatServerApplication.Reponsitories;
using ChatServerApplication.Uilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Services
{
    public class GroupService : IGroupService
    {
        DataStorage dataStorage;
        AccessCodeGenerator accessCodeGenerator;

        public GroupService()
        {
            dataStorage = DataStorage.GetDataStorage();
            accessCodeGenerator = new AccessCodeGenerator();
        }

        public void CreateGroup(string groupName, string groupType, User groupAdmin)
        {
            Group group;
            if (groupType.Equals("Private"))
            {
                group = new PrivateGroup(groupName, groupAdmin);
            }
            else
            {
                string accessCode = accessCodeGenerator.GetNextAccessCode();
                group = new PublicGroup(groupName, accessCode);
            }
            dataStorage.Groups.Insert(group);
        }

        public bool JoinGroupChatByAccessCode(Group group, string accessCode, User user)
        {
            if (group.GetType() == typeof(PublicGroup))
            {
                PublicGroup publicGroup = (PublicGroup)group;
                if (accessCode.Equals(publicGroup.AccessCode))
                {
                    publicGroup.Members.Add(user);
                    return true;
                }
            }
            return false;
        }

        public bool InviteMemberToGroupChat(Group group, User invitor, User member)
        {
            bool isGroupsContainMember = group.Members.Contains(member);
            bool isGroupsContainInvitor = group.Members.Contains(invitor);
            if (!isGroupsContainMember && isGroupsContainInvitor)
            {
                if (group.GetType() == typeof(PublicGroup))
                {
                    group = (PublicGroup)group;
                    group.Members.Add(member);
                    return true;
                }
                else if (group.GetType() == typeof(PrivateGroup))
                {
                    PrivateGroup privateGroup = (PrivateGroup)group;
                    bool isAdmin = privateGroup.Admins.Contains(invitor);
                    if (isAdmin)
                    {
                        privateGroup.Members.Add(member);
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Attachment> FindAllFiles(Guid senderID, Guid receiverID)
        {
            List<Message> groupMessagesContainingFile = dataStorage.Messages.Get(x => x.IsRelatedToGroup(senderID, receiverID) && x.Attachments != null).ToList();

            List<Attachment> attachments = new List<Attachment>();
            groupMessagesContainingFile.ForEach(message => attachments.AddRange(message.Attachments));

            return attachments;
        }

        public void SetAdmin(User admin, PrivateGroup privateGroup)
        {
            bool isAdmin = privateGroup.Admins.Contains(admin);
            bool isMember = privateGroup.Members.Contains(admin);

            if (!isMember)
            {
                privateGroup.Members.Add(admin);
                privateGroup.Admins.Add(admin);
            }
            else if (!isAdmin)
            {
                privateGroup.Admins.Add(admin);
            }

        }

        public bool SetAdmin1(User admin, PrivateGroup privateGroup)
        {
            bool isAdmin = privateGroup.Admins.Contains(admin);
            bool isMember = privateGroup.Members.Contains(admin);
            if (isAdmin)
            {
                if (!isMember)
                {
                    privateGroup.Members.Add(admin);
                    privateGroup.Admins.Add(admin);
                }
                else if (!isAdmin)
                {
                    privateGroup.Admins.Add(admin);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
