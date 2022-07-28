using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    internal class FriendInvitation
    {
       public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
    }
}
