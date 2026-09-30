using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class Feature2Manager : GenericManager<Feature2>, IFeatur2Servic
    {
        public Feature2Manager(IFeature2DAL dal) : base(dal) { }

    }
}
