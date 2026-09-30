using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;

namespace BusinessLayer.Concrate
{
    /// <summary>
    /// Basis-Manager: leitet die Standard-CRUD-Operationen an die DAL weiter.
    /// Spezielle Manager erben davon und ergänzen nur ihre eigene Geschäftslogik.
    /// </summary>
    public abstract class GenericManager<T> : IGenerik_Service<T> where T : class
    {
        protected readonly IGenerikDAL<T> _dal;

        protected GenericManager(IGenerikDAL<T> dal)
        {
            _dal = dal;
        }

        public virtual void Insert(T entity) => _dal.Insert(entity);
        public virtual void Update(T entity) => _dal.Updater(entity);
        public virtual void Delete(T entity) => _dal.Delete(entity);
        public virtual List<T> GetAll() => _dal.GetList();
        public virtual T GetById(int id) => _dal.Get(id);
        public virtual int Count() => _dal.Count();
    }
}
