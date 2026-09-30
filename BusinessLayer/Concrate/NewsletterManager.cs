using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class NewsletterManager : GenericManager<Newsletter>, INewsletterService
    {
        public NewsletterManager(INewsLetterDAL dal) : base(dal) { }

        public bool Subscribe(string mail)
        {
            mail = (mail ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(mail) || _dal.Count(n => n.Mail == mail) > 0)
                return false;
            _dal.Insert(new Newsletter { Mail = mail, SubscribeDate = DateTime.Now });
            return true;
        }
    }
}
