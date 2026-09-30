using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    /// <summary>Startbanner mit Schnellsuche nach Reisezielen.</summary>
    public class _SliderPartial : ViewComponent
    {
        private readonly IDestinitionServic _destination;

        public _SliderPartial(IDestinitionServic destination)
        {
            _destination = destination;
        }

        public IViewComponentResult Invoke() => View(_destination.GetActive().OrderBy(d => d.City).ToList());
    }
}
