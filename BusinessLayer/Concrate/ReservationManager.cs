using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class ReservationManager : GenericManager<Reservition>, IReservationService
    {
        private readonly IReservationDal _reservationDal;

        public ReservationManager(IReservationDal dal) : base(dal)
        {
            _reservationDal = dal;
        }

        public override void Insert(Reservition entity)
        {
            if (entity.ReservEnd < entity.ReservStart)
                throw new ArgumentException("Das Enddatum darf nicht vor dem Startdatum liegen.");
            if (entity.HowmanyPapel < 1)
                throw new ArgumentException("Mindestens eine Person ist erforderlich.");
            entity.status ??= ReservationStatus.Pending;
            base.Insert(entity);
        }

        public List<Reservition> GetlistByuserid(int userid) =>
            _reservationDal.GetlistbyUserId(userid).Where(x => x.status == ReservationStatus.Pending).ToList();

        public List<Reservition> GetlistByuseridaccept(int userid) =>
            _reservationDal.GetlistbyUserId(userid).Where(x => x.status == ReservationStatus.Approved).ToList();

        public List<Reservition> GetlistByuseridcanceld(int userid) =>
            _reservationDal.GetlistbyUserId(userid).Where(x => x.status == ReservationStatus.Canceled).ToList();

        public List<Reservition> GetListWhitDestination() => _reservationDal.Getlistwhitdesetination();

        public List<Reservition> GetListByStatus(string status) => _reservationDal.GetListByStatus(status);

        public Reservition GetWithDetails(int id) => _reservationDal.GetWithDetails(id);

        public int CountByStatus(string status) => _reservationDal.Count(r => r.status == status);

        public void ChangeStatus(int id, string status)
        {
            if (!ReservationStatus.All.Contains(status))
                throw new ArgumentException("Unbekannter Status.");
            var r = _reservationDal.Get(id);
            if (r == null) return;
            r.status = status;
            _reservationDal.Updater(r);
        }
    }
}
