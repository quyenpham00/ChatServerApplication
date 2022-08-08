using ChatServerApplication.Uilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class User
    {
        public Guid Id { get; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName
        {
            get => (LastName + " " + FirstName);
        }
        internal string HashPassword { private get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public List<User> Friends { get; }

        public User(string username, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            HashPassword = HashingPassword(password);
            Friends = new List<User>();
        }

        private string HashingPassword(string password)
        {
            PasswordEncoder passwordEncoder = new PasswordEncoder();
            passwordEncoder.HashingPassword(password);
            return passwordEncoder.ToString();
        }

        public bool CheckPassword(string password)
        {
            string passwordHash = HashingPassword(password);
            return passwordHash.Equals(HashPassword);
        }
    }
}
