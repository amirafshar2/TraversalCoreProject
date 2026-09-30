using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfDestinitionDAL : GenericRepository<Destiniton>, IDestinationDAL
    {
        public EfDestinitionDAL(Context context) : base(context) { }

        public List<Destiniton> GetallWhitTourlider()
        {
            return _context.destinitons.AsNoTracking().ToList();
        }

        public Destiniton GetWithDetails(int id)
        {
            return _context.destinitons.AsNoTracking()
                .Include(d => d.Comments)
                .Include(d => d.reservitions)
                .FirstOrDefault(d => d.DestinationID == id);
        }
    }
}
