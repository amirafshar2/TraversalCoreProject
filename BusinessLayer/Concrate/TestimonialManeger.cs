using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;

namespace BusinessLayer.Concrate
{
    public class TestimonialManeger : GenericManager<Testimonial>, ITestimonialServic
    {
        public TestimonialManeger(ITestimonialDAL dal) : base(dal) { }

        public List<Testimonial> GetActive() => _dal.GetListByFilter(t => t.Status);
    }
}
