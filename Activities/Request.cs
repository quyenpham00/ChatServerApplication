using ChatServerApplication.Models;
using ChatServerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Activities
{
    public class Request
    {
        public Guid Id { get; set; }
        public RequestStatus Status { get; set; }
        public User Sender { get; set; }
        public User Reveiver { get; set; }
        public DateTime Created { get; set; }
        public Request(User sender, User reveiver)
        {
            Id = Guid.NewGuid();
            Status = RequestStatus.Pending;
            Sender = sender;
            Reveiver = reveiver;
            Created = DateTime.Now;
        }
    }
}
