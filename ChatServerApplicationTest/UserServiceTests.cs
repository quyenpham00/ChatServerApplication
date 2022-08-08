using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatServerApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServerApplication.Models;
using ChatServerApplication.Data;
using System.Net.Mail;
using System.Net.Mime;
using Attachment = System.Net.Mail.Attachment;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http.Internal;

namespace ChatServerApplication.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        private UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService();
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            User user = new User("user1", "123");
            Assert.IsTrue(_userService.CreateUser(user));
        }

        [TestMethod()]
        public void NotCreateUserTest()
        {
            User user = new User("user1", "123");
            Assert.IsFalse(_userService.CreateUser(user));
        }

        [TestMethod()]
        public void LoginSuccessfulTest()
        {
            Assert.IsTrue(_userService.Login("user1", "123"));
        }

        [TestMethod()]
        public void LoginUnsuccessfulTest()
        {
            Assert.IsFalse(_userService.Login("user", "123"));
        }

        [TestMethod()]
        public void FindFriendsTest()
        {
            User sender = new User("user2", "456");
            User receiver = new User("user3", "789");
            receiver.LastName = "Tester";

            _userService.CreateUser(sender);
            _userService.CreateUser(receiver);

            _userService.SendAddFriendRequest(sender, receiver);
            _userService.AcceptAddFriendRequest(receiver, sender);
            List<User> friends = _userService.FindFriends(sender, "est");

            Assert.AreEqual(1, friends.Count);
        }

        [TestMethod()]
        public void SendMessageSuccessfulTest()
        {
            User sender = new User("user122", "456");
            User receiver = new User("user223", "789");
            _userService.CreateUser(sender);
            _userService.CreateUser(receiver);

            Assert.IsTrue(_userService.SendMessage(sender.Id, receiver.Id, "Hi, I'm a tester!!"));
        }

        [TestMethod()]
        public void SendMessageUnsuccessfulTest()
        {
            User sender = new User("user12", "452456");
            User receiver = new User("user23", "782879");
            _userService.CreateUser(sender);
            _userService.CreateUser(receiver);

            Assert.IsFalse(_userService.SendMessage(sender.Id, receiver.Id, ""));
        }

        [TestMethod()]
        public void UploadFiles()
        {
            string fileName = ".txt";
            string content = "kdsjH5564hjgfGYKS53415";
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            List<IFormFile> formFiles = new List<IFormFile>();

            IFormFile formFile = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: fileName
            );
            formFiles.Add(formFile);

            Assert.AreEqual(1, formFiles.Count);
        }

        [TestMethod()]
        public void SendFileSuccessfulTest()
        {
            string fileName = ".txt";
            string content = "kdsjH5564hsgfdjgfGYKS53415";
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            List<IFormFile> formFiles = new List<IFormFile>();

            IFormFile formFile = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: fileName
            );
            formFiles.Add(formFile);

            User sender = new User("sender123", "452456");
            User receiver = new User("receiver123", "782879");
            _userService.CreateUser(sender);
            _userService.CreateUser(receiver);

            Assert.IsTrue(_userService.SendFile(sender.Id, receiver.Id, formFiles));
        }

        [TestMethod()]
        public void SendFileUnsuccessfulTest()
        {
            string fileName = ".txt";
            string content = "kdsjH5564hjgfGYfdgKS53415";
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            List<IFormFile> formFiles = new List<IFormFile>();

            IFormFile formFile = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: fileName
            );
            formFiles.Add(formFile);

            User sender = new User("sender13", "452456");
            User receiver = new User("receiver13", "782879");
            _userService.CreateUser(sender);

            Assert.IsFalse(_userService.SendFile(sender.Id, receiver.Id, formFiles));
        }


        [TestMethod()]
        public void DeleteMessageTest()
        {
            User sender = new User("user678", "45678");
            User receiver = new User("user345", "78910");
            _userService.CreateUser(sender);
            _userService.CreateUser(receiver);

            _userService.SendMessage(sender.Id, receiver.Id, "Hi, I'm a tester!!");
            _userService.SendMessage(sender.Id, receiver.Id, "Hi, everyone!!");
            _userService.SendMessage(sender.Id, receiver.Id, "Yes, I'm here!!");

            Message message = _userService.FindMessages(sender.Id, receiver.Id, "Hi, I'm a tester!!")[0];
            _userService.DeleteMessage(message);
            List<Message> messages = _userService.FindMessages(receiver.Id, sender.Id, "Hi, I'm a tester!!");

            Assert.AreEqual(0, messages.Count);
        }

        [TestMethod()]
        public void GetTopLatestMessagesTest()
        {
            User sender = new User("user102", "456");
            User receiver = new User("user103", "789");
            _userService.CreateUser(sender);
            _userService.CreateUser(receiver);

            _userService.SendMessage(sender.Id, receiver.Id, "Hi, I'm a tester!!");
            _userService.SendMessage(sender.Id, receiver.Id, "Hi, everyone!!");
            _userService.SendMessage(sender.Id, receiver.Id, "Yes, I'm here!!");
            _userService.SendMessage(sender.Id, receiver.Id, "There is a Thai restaurant.");
            _userService.SendMessage(sender.Id, receiver.Id, "It's nice.");
            List<Message> messages = _userService.GetTopLatestMessages(sender.Id, receiver.Id, 3, 1);

            Assert.AreEqual(3, messages.Count);
        }

        [TestMethod()]
        public void FindMessagesExistedTest()
        {
            User sender = new User("user202", "45645");
            User receiver = new User("user203", "78459");
            _userService.CreateUser(sender);
            _userService.CreateUser(receiver);

            _userService.SendMessage(sender.Id, receiver.Id, "Yes, I'm here!!");
            _userService.SendMessage(sender.Id, receiver.Id, "There is a Thai restaurant.");
            List<Message> messages = _userService.FindMessages(sender.Id, receiver.Id, "Here");

            Assert.AreEqual(2, messages.Count);
        }

        [TestMethod()]
        public void FindMessagesNotExistedTest()
        {
            User sender = new User("user2", "456");
            User receiver = new User("user3", "789");

            _userService.SendMessage(sender.Id, receiver.Id, "");
            List<Message> messages = _userService.FindMessages(sender.Id, receiver.Id, "Not");

            Assert.AreEqual(0, messages.Count);
        }

        [TestMethod()]
        public void GetContactsOfUserTest()
        {
            User user = new User("user1232", "435456");
            _userService.CreateUser(user);
            for (int i = 0; i < 3; i++)
            {
                User friend = new User("user1354" + i, "784539" + i);
                _userService.CreateUser(friend);
                _userService.SendMessage(user.Id, friend.Id, "Yes, I'm here " + i);
            }

            Assert.AreEqual(3, _userService.GetContactsOfUser(user).Count - 1);
        }

        [TestMethod()]
        public void SetAliasSuccessfulTest()
        {
            User assignor = new User("user2", "456");
            User assignee = new User("user3", "789");
            Assert.IsTrue(_userService.SetAlias(assignor, assignee, "Test"));
        }

        [TestMethod()]
        public void SetAliasUnsuccessfulTest()
        {
            User assignor = new User("user2", "456");
            User assignee = new User("user3", "789");
            Assert.IsFalse(_userService.SetAlias(assignor, assignee, ""));
        }
    }
}