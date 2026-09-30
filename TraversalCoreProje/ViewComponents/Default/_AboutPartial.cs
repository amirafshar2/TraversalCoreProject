using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    public class _AboutPartial : ViewComponent
    {
        private readonly IAbout2Service _iabout2;

        public _AboutPartial(IAbout2Service iabout2)
        {
            _iabout2 = iabout2;
        }

        public IViewComponentResult Invoke() => View(_iabout2.GetAll().LastOrDefault());
    }
}
