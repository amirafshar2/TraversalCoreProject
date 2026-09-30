using EntityLayer.Concrate;

namespace BusinessLayer.Abstract
{
    public interface ITestimonialServic : IGenerik_Service<Testimonial>
    {
        List<Testimonial> GetActive();
    }
}
