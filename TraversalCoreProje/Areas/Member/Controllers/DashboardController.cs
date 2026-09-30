using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Areas.Member.Models;

namespace TraversalCoreProje.Areas.Member.Controllers
{
    /// <summary>Startseite des Kundenbereichs.</summary>
    [Area("Member")]
    public class DashboardController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IReservationService _reservation;
        private readonly ICommentService _comment;
        private readonly IDestinitionServic _destination;

        public DashboardController(UserManager<User> userManager, IReservationService reservation,
            ICommentService comment, IDestinitionServic destination)
        {
            _userManager = userManager;
            _reservation = reservation;
            _comment = comment;
            _destination = destination;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "User", new { area = "" });

            var all = _reservation.GetListWhitDestination().Where(r => r.Userid == user.Id).ToList();
            var bookedIds = all.Select(r => r.Destintionid).ToHashSet();

            var model = new MemberDashboardViewModel
            {
                User = user,
                PendingCount = all.Count(r => r.status == ReservationStatus.Pending),
                ApprovedCount = all.Count(r => r.status == ReservationStatus.Approved),
                CanceledCount = all.Count(r => r.status == ReservationStatus.Canceled),
                CommentCount = _comment.GetCommentsByUserID(user.Id).Count,
                NextTrip = all.Where(r => r.status == ReservationStatus.Approved && r.ReservStart >= DateTime.Today)
                              .OrderBy(r => r.ReservStart).FirstOrDefault(),
                Latest = all.OrderByDescending(r => r.ReservDate).Take(5).ToList(),
                Suggestions = _destination.GetActive().Where(d => !bookedIds.Contains(d.DestinationID))
                                          .OrderBy(_ => Guid.NewGuid()).Take(3).ToList()
            };
            return View(model);
        }
    }
}
