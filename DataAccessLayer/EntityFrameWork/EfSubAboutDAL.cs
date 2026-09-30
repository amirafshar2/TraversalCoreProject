using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfSubAboutDAL : GenericRepository<SubAbout>, ISababoutDAL
    {
        public EfSubAboutDAL(Context context) : base(context) { }

    }
}
