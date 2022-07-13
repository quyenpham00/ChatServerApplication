using ChatServerApplication.Models;
using ChatServerApplication.Reponsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Data
{
    internal class DataStorage
    {
        public Repository<User> Users { get; }
        private DataStorage storage { get; set; }
        private DataStorage()
        {
            Users = new Repository<User>();
        }
        public DataStorage GetDataStorage()
        {
            if (storage == null)
            {
                storage = new DataStorage();
            }
            return storage;
        }
    }
}
