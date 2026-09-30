using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class ContactMessageManager : GenericManager<ContactMessage>, IContactMessageService
    {
        public ContactMessageManager(IContactMessageDAL dal) : base(dal) { }

        public List<ContactMessage> GetLatest(int count) =>
            _dal.GetList().OrderByDescending(m => m.MessageDate).Take(count).ToList();

        public int CountUnread() => _dal.Count(m => !m.IsRead);

        public void MarkAsRead(int id)
        {
            var msg = _dal.Get(id);
            if (msg != null && !msg.IsRead)
            {
                msg.IsRead = true;
                _dal.Updater(msg);
            }
        }
    }
}
