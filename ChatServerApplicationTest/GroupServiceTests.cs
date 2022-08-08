using ChatServerApplication.Models;
using ChatServerApplication.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.ChatServerApplicationTest
{
    [TestClass()]
    public class GroupServiceTests
    {
        private GroupService groupService;

        public GroupServiceTests()
        {
            groupService = new GroupService();
        }

        [TestMethod()]
        public void CreateGroupTestFist()
        {
            string groupName = "Intern";
            string groupType = "Private";
            User groupAdmin = new User("userName", "123");

            try
            {
                groupService.CreateGroup(groupName, groupType, groupAdmin);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod()]
        public void CreateGroupTestSecond()
        {
            string groupName = "Intern";
            string groupType = "Private";
            User groupAdmin = new User("userName", "123");

            try
            {
                groupService.CreateGroup(groupName, groupType, groupAdmin);
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void JoinGroupChatByAccessCodeTestTrue()
        {
            string groupName = "Intern";
            User groupAdmin = new User("userName", "123");

            PrivateGroup groupPrivate = new PrivateGroup(groupName, groupAdmin);
            string accessCode = "123";
            User groupMember = new User(accessCode, "123");

            Assert.IsFalse(groupService.JoinGroupChatByAccessCode(groupPrivate, accessCode, groupMember));
        }

        [TestMethod()]
        public void JoinGroupChatByAccessCodeTestFalse()
        {
            PublicGroup groupPublic = new PublicGroup("Intern", "123");
            string accessCode = "123";
            User groupMember = new User(accessCode, "123");

            Assert.IsTrue(groupService.JoinGroupChatByAccessCode(groupPublic, accessCode, groupMember));
        }

        [TestMethod()]
        public void FindAllFilesTest()
        {
            PublicGroup groupPublic = new PublicGroup("Intern", "123");
            string groupName = "Intern";
            User groupAdmin = new User("userName", "123");
            PrivateGroup groupPrivate = new PrivateGroup(groupName, groupAdmin);
            Guid sender = groupPublic.Id;
            Guid reciver = groupAdmin.Id;

            CollectionAssert.AllItemsAreNotNull(groupService.FindAllFiles(sender, reciver));
        }

        static User invitor = new User("Invitor", "123");
        static User member = new User("Member", "321");
        static PublicGroup groupPublic1 = new PublicGroup("groupMain", "123");

        [TestMethod()]
        public void InviteMemberToGroupTestPublicTrue()
        {
            groupPublic1.Members.Add(invitor);
            Assert.IsTrue(groupService.InviteMemberToGroupChat(groupPublic1, invitor, member));
        }

        [TestMethod()]
        public void InviteMemberToGroupTestPublicFalse()
        {
            Assert.IsFalse(groupService.InviteMemberToGroupChat(groupPublic1, invitor, member));
        }

        static PrivateGroup groupPrivate1 = new PrivateGroup("groupMain", invitor);

        [TestMethod()]
        public void InviteMemberToGroupTestPrivateTrue()
        {
            groupPrivate1.Members.Add(invitor);
            Assert.IsTrue(groupService.InviteMemberToGroupChat(groupPrivate1, invitor, member));
        }

        static User groupPrivateAdmin = new User("Adin", "123");
        static PrivateGroup groupPrivate2 = new PrivateGroup("groupMain", groupPrivateAdmin);

        [TestMethod()]
        public void InviteMemberToGroupTestPrivateFalse()
        {
            Assert.IsFalse(groupService.InviteMemberToGroupChat(groupPrivate2, invitor, member));
        }

        [TestMethod()]
        public void SetAdminTestFirst()
        {
            User admin = new User("Admin", "123");
            User member = new User("Member", "123");
            PrivateGroup privateGroup = new PrivateGroup("privateGroup", admin);
            groupService.SetAdmin(member, privateGroup);

            bool result = privateGroup.Admins.Contains(member);
            try
            {
                Assert.IsTrue(result);
            }
            catch
            {
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void SetAdminTestSecond()
        {
            User admin = new User("Admin", "123");
            User member = new User("Member", "123");
            PrivateGroup privateGroup = new PrivateGroup("privateGroup", admin);
            try
            {
                groupService.SetAdmin(member, privateGroup);
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}