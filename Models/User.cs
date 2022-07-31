using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName
        {
            get => (LastName + " " + FirstName);
        }
        public string HashPassword { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public List<User> Friends { get; set; }
        public User(string username, string lastName, string firstName, string hashPassword, Gender gender, DateOnly dateOfBirth)
        {
            Id = Guid.NewGuid();
            Username = username;
            LastName = lastName;
            FirstName = firstName;
            HashPassword = hashPassword;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Friends = new List<User>();
        }
    }
}
