using EntityLayer.Concrate;

namespace BusinessLayer.Abstract
{
    public interface IContactMessageService : IGenerik_Service<ContactMessage>
    {
        List<ContactMessage> GetLatest(int count);
        int CountUnread();
        void MarkAsRead(int id);
    }
}
