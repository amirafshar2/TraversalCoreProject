using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfNewsletterDAL : GenericRepository<Newsletter>, INewsLetterDAL
    {
        public EfNewsletterDAL(Context context) : base(context) { }

    }
}
