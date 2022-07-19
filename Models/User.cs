using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName
        {
            get => (LastName + " " + FirstName);
        }
        public string HashPassword { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<User> Friends { get; set; }
        public ICollection<FriendInvitation> FriendInvitation { get; set; }
    }
}
