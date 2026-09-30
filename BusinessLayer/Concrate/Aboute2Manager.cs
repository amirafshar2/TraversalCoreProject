using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class Aboute2Manager : GenericManager<About2>, IAbout2Service
    {
        public Aboute2Manager(IAbout2DAL dal) : base(dal) { }

    }
}
