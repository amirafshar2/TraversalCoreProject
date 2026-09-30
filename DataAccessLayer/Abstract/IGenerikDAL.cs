using System.Linq.Expressions;

namespace DataAccessLayer.Abstract
{
    public interface IGenerikDAL<T> where T : class
    {
        void Insert(T t);
        void Delete(T t);
        void Updater(T t);
        List<T> GetList();
        List<T> GetListByFilter(Expression<Func<T, bool>> filter);
        T Get(int id);
        int Count(Expression<Func<T, bool>> filter = null);
    }
}
