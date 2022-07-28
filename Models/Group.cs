using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public List<User> Members { get; set; }
        public DateTime Created { get; set; }
        public List <Message> Messages { get; set; }
        public Group(String name = "")
        {
            Id = Guid.NewGuid();
            Name = name;
            Members = new List<User>();
            Created = DateTime.Now;
        }
    }
}
