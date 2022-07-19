using ChatServerApplication.Models;
using ChatServerApplication.Reponsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Services
{
    public class UserService
    {
        Repository<User> users = new Repository<User>();
        public void Login(String username, String password)
        {
            

        }
        public static List<User> GetAllUser()
        {
            List<User> users = new List<User>();
            return users;
        }
        public static User GetUser(int id)
        {
            List<User> list = new List<User>();
            User user = null;
            foreach(User temp in list)
            {
                if (temp.Id == id)
                {
                    user=temp;
                }
            }
            return user;
        }
        public static int FindFriend(string FriendUsername)
        {
            List<User> list = GetAllUser();
            foreach(User user in list)
            {
                if (user.Username.Contains(FriendUsername))
                {
                    Console.WriteLine(user.Username+" "+user.Id);
                }
                           }
            Console.WriteLine("Choose Friend Id");
            string id =Console.ReadLine();
       
            return int.Parse(id);
        }
        public static string SendInvitation()
        {
            string result = "";
            int userId = 1;
            Console.WriteLine("Input friend user:");
            string friendName=Console.ReadLine();
            int friendId=FindFriend(friendName);
            Console.WriteLine(" Say Friend Invitation:");
            string invitation = Console.ReadLine();
            User user=GetUser(friendId);

            FriendInvitation friendInvitation=new FriendInvitation()
            {

                UserId=userId,
                FriendId=friendId,
                DateTime=DateTime.Now,
                Description=invitation,

            };
            try
            {
                user.FriendInvitation.Add(friendInvitation);
                result = "Ok";
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }
    }
}
