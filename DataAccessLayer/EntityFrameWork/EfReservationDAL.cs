using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfReservationDAL : GenericRepository<Reservition>, IReservationDal
    {
        public EfReservationDAL(Context context) : base(context) { }

        public List<Reservition> GetlistbyUserId(int userId)
        {
            return _context.reservitions.AsNoTracking()
                .Where(x => x.Userid == userId)
                .Include(x => x.Destiniton)
                .Include(x => x.Guide)
                .ToList();
        }

        public List<Reservition> Getlistwhitdesetination()
        {
            return _context.reservitions.AsNoTracking()
                .Include(x => x.Destiniton)
                .Include(x => x.User)
                .Include(x => x.Guide)
                .ToList();
        }

        public List<Reservition> GetListByStatus(string status)
        {
            return _context.reservitions.AsNoTracking()
                .Where(x => x.status == status)
                .Include(x => x.Destiniton)
                .Include(x => x.User)
                .Include(x => x.Guide)
                .ToList();
        }

        public Reservition GetWithDetails(int id)
        {
            return _context.reservitions.AsNoTracking()
                .Include(x => x.Destiniton)
                .Include(x => x.User)
                .Include(x => x.Guide)
                .FirstOrDefault(x => x.id == id);
        }
    }
}
