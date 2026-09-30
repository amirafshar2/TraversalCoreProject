using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfFeatureDAL : GenericRepository<Feature>, IFeatureDAL
    {
        public EfFeatureDAL(Context context) : base(context) { }

    }
}
