using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Default
{
    /// <summary>Kennzahlen auf der Startseite – live aus der Datenbank.</summary>
    public class _StatsPartial : ViewComponent
    {
        private readonly IDestinitionServic _destination;
        private readonly IGuideService _guide;
        private readonly IUserService _user;
        private readonly IReservationService _reservation;

        public _StatsPartial(IDestinitionServic destination, IGuideService guide, IUserService user, IReservationService reservation)
        {
            _destination = destination;
            _guide = guide;
            _user = user;
            _reservation = reservation;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.DestinitionsCount = _destination.Count();
            ViewBag.GuideCount = _guide.Count();
            ViewBag.Costumer = _user.Count();
            ViewBag.Trips = _reservation.CountByStatus(ReservationStatus.Approved);
            return View();
        }
    }
}
