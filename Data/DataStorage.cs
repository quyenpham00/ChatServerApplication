using ChatServerApplication.Activities;
using ChatServerApplication.Models;
using ChatServerApplication.Reponsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Data
{
    public class DataStorage
    {
        public Repository<User> Users { get; }
        public Repository<Group> Groups { get; }
        public Repository<Message> Messages { get; }
        public Repository<Request> Requests { get; }
        public DataStorage Storage { get; set; }
        public DataStorage()
        {
            Users = new Repository<User>();
            Groups = new Repository<Group>();
            Messages = new Repository<Message>();
            Requests = new Repository<Request>();
        }
        public DataStorage GetDataStorage()
        {
            if (Storage == null)
            {
                Storage = new DataStorage();
            }
            return Storage;
        }
    }
}
