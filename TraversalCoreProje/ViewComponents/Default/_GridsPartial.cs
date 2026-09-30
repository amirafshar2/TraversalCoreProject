using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    /// <summary>Beliebte Reiseziele auf der Startseite.</summary>
    public class _GridsPartial : ViewComponent
    {
        private readonly IDestinitionServic _destination;

        public _GridsPartial(IDestinitionServic destination)
        {
            _destination = destination;
        }

        public IViewComponentResult Invoke()
        {
            return View(_destination.GetActive().OrderByDescending(d => d.Price).Take(6).ToList());
        }
    }
}
