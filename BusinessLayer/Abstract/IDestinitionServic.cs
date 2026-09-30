using EntityLayer.Concrate;

namespace BusinessLayer.Abstract
{
    public interface IDestinitionServic : IGenerik_Service<Destiniton>
    {
        List<Destiniton> GetWhitTourlider();
        List<Destiniton> GetActive();
        Destiniton GetWithDetails(int id);
        void ToggleStatus(int id, bool status);
    }
}
