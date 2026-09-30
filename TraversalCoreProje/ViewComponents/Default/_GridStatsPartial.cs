using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    /// <summary>Top-Angebote (Feature) auf der Startseite: ein großes und bis zu vier kleine.</summary>
    public class _GridStatsPartial : ViewComponent
    {
        private readonly IFeatureServic _featureManager;

        public _GridStatsPartial(IFeatureServic featureManager)
        {
            _featureManager = featureManager;
        }

        public IViewComponentResult Invoke()
        {
            var active = _featureManager.GetAll().Where(f => f.Status).ToList();
            var first = active.FirstOrDefault();
            ViewBag.First = first;
            return View(active.Skip(1).Take(4).ToList());
        }
    }
}
