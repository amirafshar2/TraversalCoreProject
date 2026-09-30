using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfContactMessageDAL : GenericRepository<ContactMessage>, IContactMessageDAL
    {
        public EfContactMessageDAL(Context context) : base(context) { }

    }
}
