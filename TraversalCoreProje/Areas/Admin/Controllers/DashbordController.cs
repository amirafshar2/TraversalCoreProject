using System.Globalization;
using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Areas.Admin.Models;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class DashbordController : Controller
    {
        private readonly IUserService _user;
        private readonly IDestinitionServic _destination;
        private readonly IReservationService _reservation;
        private readonly ICommentService _comment;
        private readonly IGuideService _guide;
        private readonly IContactMessageService _message;
        private readonly INewsletterService _newsletter;

        public DashbordController(IUserService user, IDestinitionServic destination, IReservationService reservation,
            ICommentService comment, IGuideService guide, IContactMessageService message, INewsletterService newsletter)
        {
            _user = user;
            _destination = destination;
            _reservation = reservation;
            _comment = comment;
            _guide = guide;
            _message = message;
            _newsletter = newsletter;
        }

        public IActionResult Index()
        {
            var reservations = _reservation.GetListWhitDestination();
            var destinations = _destination.GetAll();
            var de = new CultureInfo("de-DE");

            var model = new DashboardViewModel
            {
                UserCount = _user.Count(),
                DestinationCount = destinations.Count,
                ActiveDestinationCount = destinations.Count(d => d.Status),
                PendingCount = reservations.Count(r => r.status == ReservationStatus.Pending),
                ApprovedCount = reservations.Count(r => r.status == ReservationStatus.Approved),
                CanceledCount = reservations.Count(r => r.status == ReservationStatus.Canceled),
                CommentCount = _comment.Count(),
                GuideCount = _guide.Count(),
                UnreadMessages = _message.CountUnread(),
                NewsletterCount = _newsletter.Count(),
                ExpectedRevenue = reservations
                    .Where(r => r.status == ReservationStatus.Approved)
                    .Sum(r => r.HowmanyPapel * (r.Destiniton?.Price ?? 0)),
                TopDestinations = reservations
                    .Where(r => r.status != ReservationStatus.Canceled && r.Destiniton != null)
                    .GroupBy(r => r.Destiniton.City)
                    .Select(g => (g.Key, g.Count()))
                    .OrderByDescending(x => x.Item2).Take(5).ToList(),
                LatestPending = reservations
                    .Where(r => r.status == ReservationStatus.Pending)
                    .OrderByDescending(r => r.ReservDate).Take(5).ToList()
            };

            // Neue Buchungen der letzten 6 Monate (nach Reisebeginn)
            var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-2);
            for (int i = 0; i < 6; i++)
            {
                var m = start.AddMonths(i);
                model.MonthLabels.Add(m.ToString("MMM yy", de));
                model.MonthValues.Add(reservations.Count(r => r.ReservStart.Year == m.Year && r.ReservStart.Month == m.Month
                                                             && r.status != ReservationStatus.Canceled));
            }

            return View(model);
        }
    }
}
