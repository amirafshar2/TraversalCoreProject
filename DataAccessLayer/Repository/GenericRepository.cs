using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository
{
    /// <summary>
    /// Generisches Repository. Der Context wird per Dependency Injection (Scoped) übergeben,
    /// damit pro HTTP-Request genau ein Context verwendet und korrekt freigegeben wird.
    /// </summary>
    public class GenericRepository<T> : IGenerikDAL<T> where T : class
    {
        protected readonly Context _context;

        public GenericRepository(Context context)
        {
            _context = context;
        }

        public void Insert(T t)
        {
            _context.Add(t);
            _context.SaveChanges();
        }

        public void Delete(T t)
        {
            _context.Remove(t);
            _context.SaveChanges();
        }

        public void Updater(T t)
        {
            _context.Update(t);
            _context.SaveChanges();
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public List<T> GetList()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public List<T> GetListByFilter(Expression<Func<T, bool>> filter)
        {
            return _context.Set<T>().AsNoTracking().Where(filter).ToList();
        }

        public int Count(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? _context.Set<T>().Count() : _context.Set<T>().Count(filter);
        }
    }
}
