using ChatServerApplication.Models;
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
        bool CreateUser(string username, string lastName, string firstName, string password, Gender gender, DateOnly dateOfBirth);
        List<User> FindFriends(User user, string friendName);
        void SendAddFriendRequest(User sender, User receiver);
        bool AcceptAddFriendRequest(User user, User friend);
        Message CheckTypeOfReceiver(Guid senderID, Guid receiverID);
    }
}
