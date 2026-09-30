using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    public class _TestimonialsPartial : ViewComponent
    {
        private readonly ITestimonialServic _testimonial;

        public _TestimonialsPartial(ITestimonialServic testimonial)
        {
            _testimonial = testimonial;
        }

        public IViewComponentResult Invoke() => View(_testimonial.GetActive());
    }
}
