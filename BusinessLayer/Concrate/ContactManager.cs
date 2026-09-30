using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class ContactManager : GenericManager<Contact>, IContactServic
    {
        public ContactManager(IContactDAL dal) : base(dal) { }

    }
}
