using ChatServerApplication.Activities;
using ChatServerApplication.Data;
using ChatServerApplication.Models;
using ChatServerApplication.Models.Enums;
using ChatServerApplication.Reponsitories;
using ChatServerApplication.Uilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Services
{
    public class UserService : IUserService
    {
        DataStorage dataStorage;
        PasswordEncoder passwordEncoder;
        public UserService()
        {
            dataStorage = DataStorage.GetDataStorage();
            passwordEncoder = new PasswordEncoder();
        }

        public bool Login(string username, string password)
        {
            string passwordHash = passwordEncoder.HashingPassword(password);
            User user = dataStorage.Users.Find(user => user.Username.Equals(username) && user.HashPassword.Equals(passwordHash));
            return user != null;
        }

        public bool CreateUser(string username, string lastName, string firstName, string password, Gender gender, DateOnly dateOfBirth)
        {
            User user = dataStorage.Users.Find(user => user.Username.Equals(username));
            bool passwordIsValid = passwordEncoder.CheckPasswordValid(password);
            if (user == null && passwordIsValid)
            {
                string passwordHash = passwordEncoder.HashingPassword(password);
                User newUser = new User(username, lastName, firstName, passwordHash, gender, dateOfBirth);
                dataStorage.Users.Insert(newUser);
                return true;
            }
            return false;
        }

        public List<User> FindFriends(User user, string friendName)
        {
            List<User> friends = user.Friends;
            List<User> friendsMatchKeyword = new List<User>();
            foreach (User friend in friends)
            {
                if (friend.FullName.Contains(friendName, StringComparison.OrdinalIgnoreCase))
                {
                    friendsMatchKeyword.Add(friend);
                }
            }
            return friendsMatchKeyword;
        }

        public void SendAddFriendRequest(User sender, User receiver)
        {
            Request request = new Request(sender, receiver);
            dataStorage.Requests.Insert(request);
        }

        public bool AcceptAddFriendRequest(User user, User friend)
        {
            Request friendRequest = dataStorage.Requests.Find(x => x.Sender == friend && x.Reveiver == user);
            if (friendRequest != null)
            {
                user.Friends.Add(friend);
                friend.Friends.Add(user);
                dataStorage.Requests.Delete(friendRequest);
                return true;
            }
            return false;
        }

        public Message CheckTypeOfReceiver(Guid senderID, Guid receiverID)
        {
            Message message = new Message(senderID, receiverID);
            User user = dataStorage.Users.Find(x => x.Id == receiverID);
            if (user == null)
            {
                Group group = dataStorage.Groups.Find(x => x.Id == receiverID);
                if (group != null)
                {
                    message.ReceiverType = Receiver.Group;
                }
            }
            else
            {
                message.ReceiverType = Receiver.User;
            }
            return message;
        }

        public void SendMessage(Guid senderID, Guid receiverID, string content)
        {
            Message message = CheckTypeOfReceiver(senderID, receiverID);
            message.Content = content;
            dataStorage.Messages.Insert(message);
        }

        private void SaveFileToFolder(IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            using (var stream = File.Create(filePath))
            {
                file.CopyTo(stream);
            }
        }

        private Attachment UploadFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                string fileName = file.FileName;
                Attachment attachment = new Attachment(fileName, fileBytes);
                return attachment;
            }
        }

        public void SendFile(Guid senderID, Guid receiverID, List<IFormFile> files)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    Attachment attachment = UploadFile(file);
                    attachments.Add(attachment);
                    SaveFileToFolder(file);
                }
            }
            Message message = CheckTypeOfReceiver(senderID, receiverID);
            message.Attachments = attachments;
            dataStorage.Messages.Insert(message);
        }

        public void DeleteMessage(Message message)
        {
            dataStorage.Messages.Delete(message);
        }

        public List<Message> GetTopLatestMessages(Guid senderID, Guid receiverID, int k, int m)
        {
            List<Message> messages = dataStorage.Messages.Get(x => x.SenderID == senderID && x.ReceiverID == receiverID).ToList();
            List<Message> topLatestMessages = new List<Message>();
            int start = messages.Count() - k - m;
            int end = messages.Count() - m;
            topLatestMessages = messages.GetRange(start, end);
            return topLatestMessages;
        }

        public List<Message> FindMessages(Guid senderID, Guid receiverID, string keyword)
        {
            List<Message> messages = dataStorage.Messages.Get(x => x.SenderID == senderID && x.ReceiverID == receiverID).ToList();
            List<Message> textMessages = messages.Where(x => x.Content != "" && x.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            return textMessages;
        }

        public List<Guid> GetGroupsOfUser(User user)
        {
            List<Group> groupOfUser = dataStorage.Groups.Get(x => x.Members.Contains(user)).ToList();
            List<Guid> groupIdOfUser = new List<Guid>();
            groupOfUser.ForEach(group => groupIdOfUser.Add(group.Id));
            return groupIdOfUser;
        }

        public List<Guid> GetContactOfUser(User user)
        {
            List<Guid> contacts = new List<Guid>();
            List<Message> messages = dataStorage.Messages.Get(x => x.SenderID == user.Id || x.ReceiverID == user.Id).ToList();
            messages.ForEach(message => { contacts.Add(message.SenderID); contacts.Add(message.ReceiverID); });
            return contacts.Distinct().ToList();
        }

        public List<Guid> GetConversations(User user)
        {
            List<Guid> groupOfUser = GetGroupsOfUser(user);
            List<Guid> contactOfUser = GetContactOfUser(user);
            List<Guid> conversations = new List<Guid>();
            conversations.AddRange(groupOfUser);
            conversations.AddRange(contactOfUser);
            return conversations;
        }

        public bool LeaveGroup(User member, Group group)
        {
            bool isGroupContainUser = group.Members.Contains(member);
            if (isGroupContainUser)
            {
                group.Members.Remove(member);
                if (group.GetType() == typeof(PrivateGroup))
                {
                    PrivateGroup privateGroup = (PrivateGroup)group;
                    if (privateGroup.Admins.Contains(member))
                    {
                        privateGroup.Admins.Remove(member);
                    }
                }
            }
            return isGroupContainUser;
        }

        public void SetAlias(User assignor, User assignee, string aliasName)
        {
            Alias alias = new Alias(aliasName, assignee, assignor);
            dataStorage.Alias.Insert(alias);
        }
    }
}