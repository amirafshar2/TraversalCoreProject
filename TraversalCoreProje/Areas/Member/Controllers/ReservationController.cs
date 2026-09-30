using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Areas.Member.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace TraversalCoreProje.Areas.Member.Controllers
{
    /// <summary>Reservierungen des angemeldeten Kunden. Jede Aktion prüft, ob die Reservierung dem Kunden gehört.</summary>
    [Area("Member")]
    public class ReservationController : Controller
    {
        private const int PageSize = 5;

        private readonly UserManager<User> _usermanager;
        private readonly IReservationService _bll;
        private readonly IDestinitionServic _destinitonbll;
        private readonly IGuideService _guide;

        public ReservationController(UserManager<User> usermanager, IReservationService bll,
            IDestinitionServic destinitonbll, IGuideService guide)
        {
            _usermanager = usermanager;
            _bll = bll;
            _destinitonbll = destinitonbll;
            _guide = guide;
        }

        #region Neue Reservierung
        [HttpGet]
        public IActionResult Index(int id)
        {
            var dest = _destinitonbll.GetById(id);
            if (dest == null || !dest.Status)
            {
                TempData["error"] = "Bitte wählen Sie zuerst ein Reiseziel aus.";
                return RedirectToAction(nameof(NewReservation));
            }
            ViewBag.Guides = _guide.GetActive();
            var start = DateTime.Today.AddDays(14);
            return View(new ReservationModel
            {
                Destintionid = dest.DestinationID,
                City = dest.City,
                DayNight = dest.DayNight,
                Price = dest.Price,
                Image = dest.Image,
                ReservStart = start,
                ReservEnd = start.AddDays(5)
            });
        }

        /// <summary>Reiseziel für eine neue Reservierung auswählen.</summary>
        public IActionResult NewReservation()
        {
            return View(_destinitonbll.GetActive());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReservation(ReservationModel p)
        {
            var user = await _usermanager.GetUserAsync(User);
            var dest = _destinitonbll.GetById(p.Destintionid);
            if (user == null || dest == null) return RedirectToAction(nameof(NewReservation));

            if (p.ReservStart.HasValue && p.ReservStart.Value.Date < DateTime.Today)
                ModelState.AddModelError(nameof(p.ReservStart), "Das Anreisedatum darf nicht in der Vergangenheit liegen.");
            if (p.ReservStart.HasValue && p.ReservEnd.HasValue && p.ReservEnd <= p.ReservStart)
                ModelState.AddModelError(nameof(p.ReservEnd), "Das Abreisedatum muss nach dem Anreisedatum liegen.");
            if (p.HowmanyPapel > dest.Capacity)
                ModelState.AddModelError(nameof(p.HowmanyPapel), $"Für dieses Reiseziel sind maximal {dest.Capacity} Personen möglich.");

            if (!ModelState.IsValid)
            {
                p.City = dest.City; p.DayNight = dest.DayNight; p.Price = dest.Price; p.Image = dest.Image;
                ViewBag.Guides = _guide.GetActive();
                return View("Index", p);
            }

            _bll.Insert(new Reservition
            {
                Destintionid = dest.DestinationID,
                Guidid = p.Guidid,
                HowmanyPapel = p.HowmanyPapel,
                ReservDate = DateTime.Now,
                ReservStart = p.ReservStart!.Value,
                ReservEnd = p.ReservEnd!.Value,
                status = ReservationStatus.Pending,
                Userid = user.Id,
                Username = $"{user.Name} {user.Surname}",
                Note = p.Note
            });
            TempData["success"] = $"Ihre Reservierung für {dest.City} wurde gesendet und wartet auf Bestätigung.";
            return RedirectToAction(nameof(GetReservations));
        }
        #endregion

        #region Listen
        public Task<IActionResult> GetReservations(int page = 1) => List(ReservationStatus.Pending, page);
        public Task<IActionResult> AcceptedReservations(int page = 1) => List(ReservationStatus.Approved, page);
        public Task<IActionResult> CanceledReservations(int page = 1) => List(ReservationStatus.Canceled, page);

        private async Task<IActionResult> List(string status, int page)
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "User", new { area = "" });

            var mine = _bll.GetListWhitDestination().Where(r => r.Userid == user.Id).ToList();
            ViewBag.CountPending = mine.Count(r => r.status == ReservationStatus.Pending);
            ViewBag.CountApproved = mine.Count(r => r.status == ReservationStatus.Approved);
            ViewBag.CountCanceled = mine.Count(r => r.status == ReservationStatus.Canceled);
            ViewBag.Status = status;
            ViewBag.Title = status == ReservationStatus.Pending ? "Ausstehende Reservierungen"
                          : status == ReservationStatus.Approved ? "Bestätigte Reservierungen" : "Stornierte Reservierungen";

            var list = mine.Where(r => r.status == status).OrderByDescending(r => r.ReservDate).ToPagedList(page, PageSize);
            return View("List", list);
        }
        #endregion

        #region Bearbeiten
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var r = await GetOwnAsync(id);
            if (r == null) return NotFound();
            if (r.status == ReservationStatus.Canceled)
            {
                TempData["error"] = "Stornierte Reservierungen können nicht bearbeitet werden.";
                return RedirectToAction(nameof(CanceledReservations));
            }
            ViewBag.Guides = _guide.GetActive();
            return View(new ReservationModel
            {
                id = r.id,
                Destintionid = r.Destintionid,
                HowmanyPapel = r.HowmanyPapel,
                ReservStart = r.ReservStart,
                ReservEnd = r.ReservEnd,
                Guidid = r.Guidid,
                Note = r.Note,
                City = r.Destiniton?.City,
                DayNight = r.Destiniton?.DayNight,
                Price = r.Destiniton?.Price ?? 0,
                Image = r.Destiniton?.Image
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ReservationModel p)
        {
            var rec = await GetOwnAsync(p.id);
            if (rec == null) return NotFound();

            if (p.ReservStart.HasValue && p.ReservEnd.HasValue && p.ReservEnd <= p.ReservStart)
                ModelState.AddModelError(nameof(p.ReservEnd), "Das Abreisedatum muss nach dem Anreisedatum liegen.");
            if (!ModelState.IsValid)
            {
                p.City = rec.Destiniton?.City; p.DayNight = rec.Destiniton?.DayNight;
                p.Price = rec.Destiniton?.Price ?? 0; p.Image = rec.Destiniton?.Image;
                ViewBag.Guides = _guide.GetActive();
                return View(p);
            }

            var entity = _bll.GetById(rec.id);
            entity.status = ReservationStatus.Pending;   // Änderungen müssen erneut bestätigt werden
            entity.HowmanyPapel = p.HowmanyPapel;
            entity.ReservStart = p.ReservStart!.Value;
            entity.ReservEnd = p.ReservEnd!.Value;
            entity.Guidid = p.Guidid;
            entity.Note = p.Note;
            _bll.Update(entity);
            TempData["success"] = "Ihre Reservierung wurde geändert und wartet erneut auf Bestätigung.";
            return RedirectToAction(nameof(GetReservations));
        }
        #endregion

        #region Stornieren
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delate(int id)
        {
            var r = await GetOwnAsync(id);
            if (r != null)
            {
                _bll.ChangeStatus(r.id, ReservationStatus.Canceled);
                TempData["success"] = "Die Reservierung wurde storniert.";
            }
            return RedirectToAction(nameof(CanceledReservations));
        }
        #endregion

        /// <summary>Lädt eine Reservierung nur, wenn sie dem angemeldeten Benutzer gehört.</summary>
        private async Task<Reservition> GetOwnAsync(int id)
        {
            var user = await _usermanager.GetUserAsync(User);
            var r = _bll.GetWithDetails(id);
            return r != null && user != null && r.Userid == user.Id ? r : null;
        }
    }
}
