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
    public class GroupService
    {
        Repository<Group> groups = new Repository<Group>();

        public Group FindGroupByID(Guid groupID)
        {
            return groups.Find(x => x.Id == groupID);
        }
        public IEnumerable<Group> FindGroupByName(String name)
        {
            return groups.Get(group => group.Name.Contains(name, StringComparison.OrdinalIgnoreCase), q => q.OrderBy(s => s.Name));
        }
        public void CreateGroup(String groupName, string groupType, User groupAdmin)
        {
            if (groupType.Equals("Private"))
            {
                PrivateGroup privateGroup = new PrivateGroup(groupName, groupAdmin);
                groups.Insert(privateGroup);
            }
            else
            {
                AccessCodeGenerator accessCodeGenerator = new AccessCodeGenerator();
                String accessCode = accessCodeGenerator.GetNextAccessCode();
                PublicGroup publicGroup = new PublicGroup(groupName, accessCode);
                groups.Insert(publicGroup);
            }
        }
        public Boolean JoinGroupChatByAccessCode(Guid groupID, String accessCode, User user)
        {
            Group group = FindGroupByID(groupID);
            if (group != null)
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
            }
            return false;
        }

        public Boolean InviteMemberToGroupChat(Guid groupID, User invitor, User user)
        {
            Group group = FindGroupByID(groupID);
            Boolean isGroupsMembers = !group.Members.Contains(user) && group.Members.Contains(invitor);
            if (group != null && isGroupsMembers)
            {
                if (group.GetType() == typeof(PublicGroup))
                {
                    PublicGroup publicGroup = (PublicGroup)group;
                    publicGroup.Members.Add(user);
                    return true;
                }
                else if (group.GetType() == typeof(PrivateGroup))
                {
                    PrivateGroup privateGroup = (PrivateGroup)group;
                    Boolean isAdmin = privateGroup.Admins.Contains(invitor);
                    if (isAdmin)
                    {
                        privateGroup.Members.Add(user);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
