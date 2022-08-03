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
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        TEntity Find(Func<TEntity, bool> predicate);
        void Insert(TEntity entity);
        void Delete(TEntity entityToDelete);
    }
}
