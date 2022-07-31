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
        public Repository<Attachment> Attachments { get; }
        public Repository<Alias> Alias { get; }
        private static DataStorage Storage { get; set; }
        protected DataStorage()
        {
            Users = new Repository<User>();
            Groups = new Repository<Group>();
            Messages = new Repository<Message>();
            Requests = new Repository<Request>();
            Attachments = new Repository<Attachment>();
            Alias = new Repository<Alias>();
        }
        public static DataStorage GetDataStorage()
        {
            if (Storage == null)
            {
                Storage = new DataStorage();
            }
            return Storage;
        }
    }
}
