using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfAbout2DAL : GenericRepository<About2>, IAbout2DAL
    {
        public EfAbout2DAL(Context context) : base(context) { }

    }
}
