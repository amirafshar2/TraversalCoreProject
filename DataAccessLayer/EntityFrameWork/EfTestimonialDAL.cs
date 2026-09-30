using DataAccessLayer.Abstract;
using DataAccessLayer.Concrate;
using DataAccessLayer.Repository;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFrameWork
{
    public class EfTestimonialDAL : GenericRepository<Testimonial>, ITestimonialDAL
    {
        public EfTestimonialDAL(Context context) : base(context) { }

    }
}
