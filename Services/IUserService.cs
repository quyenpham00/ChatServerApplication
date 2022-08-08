using ChatServerApplication.Models;
using ChatServerApplication.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Services
{
    public interface IUserService
    {
        bool Login(string username, string password);
        bool CreateUser(User newUser);
        List<User> FindFriends(User user, string friendName);
        void SendAddFriendRequest(User sender, User receiver);
        bool AcceptAddFriendRequest(User user, User friend);
        Receiver GetTypeOfReceiver(Guid receiverID);
        bool SendMessage(Guid senderID, Guid receiverID, string content);
        List<Attachment> UploadFiles(List<IFormFile> files);
        bool SendFile(Guid senderID, Guid receiverID, List<IFormFile> files);
        void DeleteMessage(Message message);
        List<Message> GetTopLatestMessages(Guid senderID, Guid receiverID, int k, int m);
        List<Message> FindMessages(Guid senderID, Guid receiverID, string keyword);
        List<Guid> GetGroupsOfUser(User user);
        List<Guid> GetConversations(User user);
        bool LeaveGroup(User member, Group group);
        bool SetAlias(User assignor, User assignee, string aliasName);
    }
}
