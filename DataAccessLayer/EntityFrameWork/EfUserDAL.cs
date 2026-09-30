using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfUserDAL : GenericRepository<User>, IUserDAL
    {
        public EfUserDAL(Context context) : base(context) { }

    }
}
