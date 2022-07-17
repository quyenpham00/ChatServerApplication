using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Reponsitories
{
    
        public interface IRepository<TEntity> where TEntity : class
        {
            IEnumerable<TEntity> Get();
            TEntity Find(int id);
            void Insert(TEntity entity);
            void Delete(TEntity entityToDelete);
        }
    
}
