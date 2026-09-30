using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class FeatureManager : GenericManager<Feature>, IFeatureServic
    {
        public FeatureManager(IFeatureDAL dal) : base(dal) { }

    }
}
