using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Reponsitories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IList<TEntity> _entities;
        public Repository()
        {
            _entities = new List<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _entities.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity Find(Func<TEntity, bool> predicate)
        {
            return _entities.FirstOrDefault(predicate);
        }

        public virtual void Insert(TEntity entity)
        {
            _entities.Add(entity);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            _entities.Remove(entityToDelete);
        }
    }
}
