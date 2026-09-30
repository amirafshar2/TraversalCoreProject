using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfCommentDAL : GenericRepository<Comment>, ICommentDAL
    {
        public EfCommentDAL(Context context) : base(context) { }

        public List<Comment> GetCommentsByDestinationID(int id)
        {
            return _context.comments.AsNoTracking()
                .Where(c => c.Destinitonid == id)
                .ToList();
        }

        public List<Comment> GetCommentsByUserID(int id)
        {
            return _context.comments.AsNoTracking()
                .Include(c => c.Destiniton)
                .Where(c => c.Userid == id)
                .ToList();
        }

        public List<Comment> GetListWithDestination()
        {
            return _context.comments.AsNoTracking()
                .Include(c => c.Destiniton)
                .ToList();
        }
    }
}
