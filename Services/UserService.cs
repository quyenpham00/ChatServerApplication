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

        public UserService()
        {
            dataStorage = DataStorage.GetDataStorage();
        }

        public bool Login(string username, string password)
        {
            User user = dataStorage.Users.Find(user => user.Username.Equals(username) && user.CheckPassword(password));
            return user != null;
        }

        public bool CreateUser(User newUser)
        {
            User user = dataStorage.Users.Find(user => user.Id == newUser.Id || user.Username == newUser.Username);
            if (user == null)
            {
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
            Request friendRequest = dataStorage.Requests.Find(x => x.Sender == friend && x.Receiver == user);
            if (friendRequest != null)
            {
                user.Friends.Add(friend);
                friend.Friends.Add(user);
                dataStorage.Requests.Delete(friendRequest);
                return true;
            }
            return false;
        }

        public Receiver GetTypeOfReceiver(Guid receiverID)
        {
            User user = dataStorage.Users.Find(x => x.Id == receiverID);
            if (user == null)
            {
                Group group = dataStorage.Groups.Find(x => x.Id == receiverID);
                if (group != null)
                {
                    return Receiver.Group;
                }
            }
            else
            {
                return Receiver.User;
            }
            return Receiver.Undefined;
        }

        public bool SendMessage(Guid senderID, Guid receiverID, string content)
        {
            User sender = dataStorage.Users.Find(user => user.Id == senderID);
            if (sender != null)
            {
                Receiver receiver = GetTypeOfReceiver(receiverID);
                if (content != "" && content != null && receiver != Receiver.Undefined)
                {
                    Message message = new Message(senderID, receiverID)
                    {
                        ReceiverType = receiver,
                        Content = content,
                    };
                    dataStorage.Messages.Insert(message);
                    return true;
                }
            }

            return false;
        }

        public List<Attachment> UploadFiles(List<IFormFile> files)
        {
            List<Attachment> uploadedFiles = new List<Attachment>();
            foreach (var f in files)
            {
                string name = f.FileName.Replace(@"\\\\", @"\\");

                if (f.Length > 0)
                {
                    var memoryStream = new MemoryStream();

                    // Check file name is valid
                    if (f.FileName.Length != 0)
                    {
                        try
                        {
                            f.CopyTo(memoryStream);

                            // Upload check if the file is less than 2mb!
                            if (memoryStream.Length < 2097152)
                            {
                                var file = new Attachment(Path.GetFileName(name), memoryStream.ToArray());
                                uploadedFiles.Add(file);
                                dataStorage.Attachments.Insert(file);
                            }
                        }
                        finally
                        {
                            memoryStream.Close();
                            memoryStream.Dispose();
                        }
                    }
                }
            }

            return uploadedFiles;
        }

        public bool SendFile(Guid senderID, Guid receiverID, List<IFormFile> files)
        {
            User sender = dataStorage.Users.Find(user => user.Id == senderID);
            if (sender != null)
            {
                List<Attachment> attachments = UploadFiles(files);
                Receiver receiver = GetTypeOfReceiver(receiverID);
                if (receiver != Receiver.Undefined && attachments.Count != 0)
                {
                    Message message = new Message(senderID, receiverID)
                    {
                        ReceiverType = receiver,
                        Attachments = attachments
                    };
                    dataStorage.Messages.Insert(message);
                    return true;
                }
            }
            return false;
        }

        public void DeleteMessage(Message message)
        {
            dataStorage.Messages.Delete(message);
        }

        public List<Message> GetTopLatestMessages(Guid senderID, Guid receiverID, int k, int m)
        {
            List<Message> messages = dataStorage
                .Messages
                .Get(user => user.SenderID == senderID && user.ReceiverID == receiverID, sort => sort.OrderBy(message => message.Created))
                .ToList();
            List<Message> topLatestMessages = new List<Message>();

            int numbersOfMessages = messages.Count();
            if (m + k <= numbersOfMessages && m >= 0 && k > 0)
            {
                int start = messages.Count() - k - m;
                int end = messages.Count() - m - 1;
                topLatestMessages = messages.GetRange(start, end);
            }
            return topLatestMessages;
        }

        public List<Message> FindMessages(Guid senderID, Guid receiverID, string keyword)
        {
            List<Message> messages = dataStorage
                .Messages
                .Get(x => x.SenderID == senderID && x.ReceiverID == receiverID)
                .ToList();
            List<Message> textMessages = messages.Where(x => x.Content != null && x.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            return textMessages;
        }

        public List<Guid> GetGroupsOfUser(User user)
        {
            List<Group> groupsOfUser = dataStorage.Groups.Get(x => x.Members.Contains(user)).ToList();
            List<Guid> groupIdOfUser = new List<Guid>();
            groupsOfUser.ForEach(group => groupIdOfUser.Add(group.Id));
            return groupIdOfUser;
        }

        public List<Guid> GetContactsOfUser(User user)
        {
            List<Guid> contacts = new List<Guid>();
            List<Message> messages = dataStorage.Messages.Get(x => x.SenderID == user.Id || x.ReceiverID == user.Id).ToList();
            messages.ForEach(message => { contacts.Add(message.SenderID); contacts.Add(message.ReceiverID); });
            return contacts.Distinct().ToList();
        }

        public List<Guid> GetConversations(User user)
        {
            List<Guid> groupOfUser = GetGroupsOfUser(user);
            List<Guid> contactsOfUser = GetContactsOfUser(user);
            List<Guid> conversations = new List<Guid>();
            conversations.AddRange(groupOfUser);
            conversations.AddRange(contactsOfUser);
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

        public bool SetAlias(User assignor, User assignee, string aliasName)
        {
            Alias alias = new Alias(aliasName, assignee, assignor);
            if (aliasName != "" && aliasName != null)
            {
                dataStorage.Alias.Insert(alias);
                return true;
            }
            return false;
        }
    }
}