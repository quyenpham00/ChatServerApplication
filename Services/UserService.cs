using ChatServerApplication.Activities;
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
    public class UserService
    {
        DataStorage dataStorage = new DataStorage();
        PasswordEncoder passwordEncoder = new PasswordEncoder();

        public User FindUserByID(Guid id)
        {
            return dataStorage.Users.Find(user => user.Id == id);
        }
        public User FindUserByUsername(String username)
        {
            return dataStorage.Users.Find(user => user.Username.Equals(username));
        }
        public IEnumerable<User> FindUserByName(String name)
        {
            return dataStorage.Users.Get(user => user.FullName.Contains(name, StringComparison.OrdinalIgnoreCase), q => q.OrderBy(s => s.FullName));
        }
        public Boolean Login(String username, String password)
        {
            String passwordHash = passwordEncoder.HashingPassword(password);
            User user = dataStorage.Users.Find(user => user.Username.Equals(username) && user.HashPassword.Equals(passwordHash));
            return user != null;
        }

        public Boolean CreateUser(string username, string lastName, string firstName, string password, Gender gender, DateTime dateOfBirth)
        {
            User user = FindUserByUsername(username);
            bool passwordIsValid = passwordEncoder.CheckPasswordValid(password);
            if (user == null && passwordIsValid)
            {
                String passwordHash = passwordEncoder.HashingPassword(password);
                User newUser = new User(username, lastName, firstName, passwordHash, gender, dateOfBirth);
                dataStorage.Users.Insert(newUser);
                return true;
            }
            return false;
        }

        public List<User> FindFriends(Guid userID, String friendName)
        {
            User user = dataStorage.Users.Find(x => x.Id == userID);
            List<User> friends = user.Friends;
            List<User> friendsMatchKeyword = new List<User>();
            foreach (User friend in friends)
            {
                if (friend.FullName.Contains(friendName, StringComparison.OrdinalIgnoreCase))
                {
                    friendsMatchKeyword.Add(friend);
                }
            }
            return friends;
        }

        public void SendAddFriendRequest(Guid senderID, Guid receiverID)
        {
            User sender = dataStorage.Users.Find(x => x.Id == senderID);
            User receiver = dataStorage.Users.Find(x => x.Id == receiverID);
            if (sender != null && receiver != null)
            {
                Request request = new Request(sender, receiver);
            }
        }

        public Boolean AcceptAddFriendRequest(Guid userID, Guid friendID)
        {
            Request friendRequest = dataStorage.Requests.Find(x => x.Sender.Equals(friendID) && x.Reveiver.Equals(userID));
            if (friendRequest != null)
            {
                User user = dataStorage.Users.Find(x => x.Id == userID);
                User friend = dataStorage.Users.Find(x => x.Id == friendID);
                if (user != null && friend != null)
                {
                    user.Friends.Add(friend);
                    friend.Friends.Add(user);
                    return true;
                }
            }
            return false;
        }

        public void SendMessage(Guid senderID, Guid groupID, String content)
        {
            User sender = FindUserByID(senderID);
            if(sender != null)
            {
                Group group = dataStorage.Groups.Find(x => x.Id.Equals(groupID));
                if(group != null)
                {
                    Message message = new Message(senderID, groupID, content);
                    group.Messages.Add(message);
                }  
            }   
        }
    }
}

