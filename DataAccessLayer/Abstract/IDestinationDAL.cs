using EntityLayer.Concrate;

namespace DataAccessLayer.Abstract
{
    public interface IDestinationDAL : IGenerikDAL<Destiniton>
    {
        List<Destiniton> GetallWhitTourlider();
        Destiniton GetWithDetails(int id);
    }
}
