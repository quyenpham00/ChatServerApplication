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
    }
}
