using EntityLayer.Concrate;

namespace DataAccessLayer.Abstract
{
    public interface IReservationDal : IGenerikDAL<Reservition>
    {
        List<Reservition> GetlistbyUserId(int userId);
        List<Reservition> Getlistwhitdesetination();
        List<Reservition> GetListByStatus(string status);
        Reservition GetWithDetails(int id);
    }
}
