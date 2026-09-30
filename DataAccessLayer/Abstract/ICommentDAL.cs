using EntityLayer.Concrate;

namespace DataAccessLayer.Abstract
{
    public interface ICommentDAL : IGenerikDAL<Comment>
    {
        List<Comment> GetCommentsByDestinationID(int id);
        List<Comment> GetCommentsByUserID(int id);
        List<Comment> GetListWithDestination();
    }
}
