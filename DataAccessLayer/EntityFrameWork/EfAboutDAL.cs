using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfAboutDAL : GenericRepository<About>, IAbouteDAL
    {
        public EfAboutDAL(Context context) : base(context) { }

    }
}
