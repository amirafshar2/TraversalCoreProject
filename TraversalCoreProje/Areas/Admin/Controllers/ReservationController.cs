using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Verwaltung aller Reservierungen: prüfen, bestätigen, stornieren.</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservation;
        private readonly IGuideService _guide;

        public ReservationController(IReservationService reservation, IGuideService guide)
        {
            _reservation = reservation;
            _guide = guide;
        }

        /// <param name="status">pending | approved | canceled | (leer = alle)</param>
        public IActionResult Index(string status, string q, int page = 1)
        {
            var list = _reservation.GetListWhitDestination().AsEnumerable();
            var statusText = MapStatus(status);
            if (statusText != null)
                list = list.Where(r => r.status == statusText);
            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim();
                list = list.Where(r => (r.Username ?? "").Contains(t, StringComparison.OrdinalIgnoreCase)
                                    || (r.Destiniton?.City ?? "").Contains(t, StringComparison.OrdinalIgnoreCase));
            }

            var all = _reservation.GetAll();
            ViewBag.Status = status;
            ViewBag.Search = q;
            ViewBag.CountAll = all.Count;
            ViewBag.CountPending = all.Count(r => r.status == ReservationStatus.Pending);
            ViewBag.CountApproved = all.Count(r => r.status == ReservationStatus.Approved);
            ViewBag.CountCanceled = all.Count(r => r.status == ReservationStatus.Canceled);

            return View(list.OrderByDescending(r => r.ReservDate).ToPagedList(page, 10));
        }

        public IActionResult Details(int id)
        {
            var r = _reservation.GetWithDetails(id);
            if (r == null) return NotFound();
            ViewBag.Guides = _guide.GetActive();
            return View(r);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id, string returnUrl)
        {
            _reservation.ChangeStatus(id, ReservationStatus.Approved);
            TempData["success"] = $"Reservierung #{id} wurde bestätigt.";
            return Back(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id, string returnUrl)
        {
            _reservation.ChangeStatus(id, ReservationStatus.Canceled);
            TempData["success"] = $"Reservierung #{id} wurde storniert.";
            return Back(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignGuide(int id, int? guideId)
        {
            var r = _reservation.GetById(id);
            if (r == null) return NotFound();
            r.Guidid = guideId;
            _reservation.Update(r);
            TempData["success"] = "Der Reiseführer wurde zugewiesen.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var r = _reservation.GetById(id);
            if (r != null)
            {
                _reservation.Delete(r);
                TempData["success"] = $"Reservierung #{id} wurde gelöscht.";
            }
            return RedirectToAction(nameof(Index));
        }

        private IActionResult Back(string returnUrl) =>
            !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction(nameof(Index));

        public static string MapStatus(string status) => status switch
        {
            "pending" => ReservationStatus.Pending,
            "approved" => ReservationStatus.Approved,
            "canceled" => ReservationStatus.Canceled,
            _ => null
        };
    }
}
