using EntityLayer.Concrate;

namespace BusinessLayer.Abstract
{
    public interface IGuideService : IGenerik_Service<Guide>
    {
        List<Guide> GetActive();
    }
}
