using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class GuideManager : GenericManager<Guide>, IGuideService
    {
        public GuideManager(IGuideDAL dal) : base(dal) { }

        public List<Guide> GetActive() => _dal.GetListByFilter(g => g.Status);
    }
}
