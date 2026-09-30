using EntityLayer.Concrate;

namespace BusinessLayer.Abstract
{
    public interface ICommentService : IGenerik_Service<Comment>
    {
        /// <summary>Nur freigeschaltete Kommentare zu einem Reiseziel.</summary>
        List<Comment> GetCommentsByDestinionID(int id);
        List<Comment> GetCommentsByUserID(int id);
        /// <summary>Alle Kommentare (auch deaktivierte) inkl. Reiseziel – für den Admin.</summary>
        List<Comment> GetAllForAdmin();
        void ToggleStatus(int id);
    }
}
