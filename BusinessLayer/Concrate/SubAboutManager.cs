using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class SubAboutManager : GenericManager<SubAbout>, ISubAboutServic
    {
        public SubAboutManager(ISababoutDAL dal) : base(dal) { }

    }
}
