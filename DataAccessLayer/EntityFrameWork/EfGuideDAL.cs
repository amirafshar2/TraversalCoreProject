using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfGuideDAL : GenericRepository<Guide>, IGuideDAL
    {
        public EfGuideDAL(Context context) : base(context) { }

    }
}
