using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    /// <summary>Reiseführer-Teaser (Startseite und „Über uns“).</summary>
    public class _GuidesPartial : ViewComponent
    {
        private readonly IGuideService _guide;

        public _GuidesPartial(IGuideService guide)
        {
            _guide = guide;
        }

        public IViewComponentResult Invoke() => View(_guide.GetActive().Take(4).ToList());
    }
}
