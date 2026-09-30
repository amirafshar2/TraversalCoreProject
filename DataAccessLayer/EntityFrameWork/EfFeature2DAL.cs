using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfFeature2DAL : GenericRepository<Feature2>, IFeature2DAL
    {
        public EfFeature2DAL(Context context) : base(context) { }

    }
}
