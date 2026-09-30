using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class CommentManager : GenericManager<Comment>, ICommentService
    {
        private readonly ICommentDAL _commentDal;

        public CommentManager(ICommentDAL dal) : base(dal)
        {
            _commentDal = dal;
        }

        /// <summary>Für die Website: nur freigeschaltete Kommentare, neueste zuerst.</summary>
        public override List<Comment> GetAll()
        {
            return _commentDal.GetList()
                .Where(x => x.status)
                .OrderByDescending(x => x.CommentData)
                .ToList();
        }

        public List<Comment> GetAllForAdmin()
        {
            return _commentDal.GetListWithDestination()
                .OrderByDescending(x => x.CommentData)
                .ToList();
        }

        public List<Comment> GetCommentsByDestinionID(int id)
        {
            return _commentDal.GetCommentsByDestinationID(id)
                .Where(i => i.status)
                .ToList();
        }

        public List<Comment> GetCommentsByUserID(int id)
        {
            return _commentDal.GetCommentsByUserID(id)
                .OrderByDescending(x => x.CommentData)
                .ToList();
        }

        public void ToggleStatus(int id)
        {
            var c = _commentDal.Get(id);
            if (c == null) return;
            c.status = !c.status;
            _commentDal.Updater(c);
        }
    }
}
