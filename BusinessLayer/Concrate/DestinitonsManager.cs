using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class DestinitonsManager : GenericManager<Destiniton>, IDestinitionServic
    {
        private readonly IDestinationDAL _destinationDal;

        public DestinitonsManager(IDestinationDAL dal) : base(dal)
        {
            _destinationDal = dal;
        }

        public List<Destiniton> GetWhitTourlider() => _destinationDal.GetallWhitTourlider();

        public List<Destiniton> GetActive() => _destinationDal.GetListByFilter(d => d.Status);

        public Destiniton GetWithDetails(int id) => _destinationDal.GetWithDetails(id);

        public void ToggleStatus(int id, bool status)
        {
            var d = _destinationDal.Get(id);
            if (d == null) return;
            d.Status = status;
            _destinationDal.Updater(d);
        }
    }
}
