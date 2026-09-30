using EntityLayer.Concrate;

namespace BusinessLayer.Abstract
{
    public interface IReservationService : IGenerik_Service<Reservition>
    {
        List<Reservition> GetlistByuserid(int userid);
        List<Reservition> GetlistByuseridcanceld(int userid);
        List<Reservition> GetlistByuseridaccept(int userid);
        List<Reservition> GetListWhitDestination();
        List<Reservition> GetListByStatus(string status);
        Reservition GetWithDetails(int id);
        void ChangeStatus(int id, string status);
        int CountByStatus(string status);
    }
}
