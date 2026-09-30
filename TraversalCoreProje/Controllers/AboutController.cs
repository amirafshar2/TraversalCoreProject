using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class AboutController : Controller
    {
        private readonly IAboutServic _about;
        private readonly ISubAboutServic _subAbout;
        private readonly IGuideService _guide;
        private readonly IDestinitionServic _destination;
        private readonly IUserService _user;
        private readonly IReservationService _reservation;

        public AboutController(IAboutServic about, ISubAboutServic subAbout, IGuideService guide,
            IDestinitionServic destination, IUserService user, IReservationService reservation)
        {
            _about = about;
            _subAbout = subAbout;
            _guide = guide;
            _destination = destination;
            _user = user;
            _reservation = reservation;
        }

        public IActionResult Index()
        {
            var model = new AboutPageViewModel
            {
                About = _about.GetAll().FirstOrDefault(a => a.Status) ?? _about.GetAll().FirstOrDefault(),
                SubAbouts = _subAbout.GetAll(),
                Guides = _guide.GetActive(),
                DestinationCount = _destination.Count(),
                CustomerCount = _user.Count(),
                ReservationCount = _reservation.CountByStatus(ReservationStatus.Approved)
            };
            return View(model);
        }
    }
}
