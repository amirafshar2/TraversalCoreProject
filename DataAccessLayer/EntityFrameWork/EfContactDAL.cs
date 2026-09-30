using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfContactDAL : GenericRepository<Contact>, IContactDAL
    {
        public EfContactDAL(Context context) : base(context) { }

    }
}
