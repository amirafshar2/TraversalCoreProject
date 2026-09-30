using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class AboutManager : GenericManager<About>, IAboutServic
    {
        public AboutManager(IAbouteDAL dal) : base(dal) { }

    }
}
