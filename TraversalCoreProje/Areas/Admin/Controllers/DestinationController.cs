using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Areas.Admin.Models;
using TraversalCoreProje.Areas.Admin.Mthods;
using TraversalCoreProje.Models.PicMethods;
using X.PagedList;
using X.PagedList.Extensions;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DestinationController : Controller
    {
        private readonly IDestinitionServic _Bll;
        private readonly IReservationService _reservation;
        private readonly UserManager<User> _usermanager;
        private readonly EntityTauchen _entity = new();   // Umwandlung Model <-> Entity
        private readonly PicSave _pic = new();            // Bilder speichern

        public DestinationController(IDestinitionServic bll, IReservationService reservation, UserManager<User> usermanager)
        {
            _Bll = bll;
            _reservation = reservation;
            _usermanager = usermanager;
        }

        #region Liste
        [HttpGet]
        public async Task<IActionResult> Index(string q, int page = 1)
        {
            const int pageSize = 8;
            var reservations = _reservation.GetAll();
            var list = _Bll.GetWhitTourlider().OrderByDescending(y => y.DestinationID).AsEnumerable();
            if (!string.IsNullOrWhiteSpace(q))
                list = list.Where(d => (d.City ?? "").Contains(q.Trim(), StringComparison.OrdinalIgnoreCase));

            var models = new List<DestinationModel>();
            foreach (var item in list)
            {
                var user = await _usermanager.FindByIdAsync(item.Turlider.ToString());
                var m = _entity.DestinitonToDestinationModel(item);
                m.username = user?.Name;
                m.usersurname = user?.Surname;
                m.userimage = user?.Image;
                m.ReservationCount = reservations.Count(r => r.Destintionid == item.DestinationID && r.status != ReservationStatus.Canceled);
                models.Add(m);
            }
            ViewBag.Search = q;
            return View(models.ToPagedList(page, pageSize));
        }
        #endregion

        #region Anlegen
        [HttpGet]
        public IActionResult Create()
        {
            return View(new DestinationModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DestinationModel p)
        {
            ValidateImages(p);
            if (p.coverImage == null)
                ModelState.AddModelError(nameof(p.coverImage), "Bitte laden Sie mindestens ein Titelbild hoch.");
            if (!ModelState.IsValid) return View(p);

            var u = await _usermanager.GetUserAsync(User);
            p.CoverImage = await _pic.SaveFileAsync(p.coverImage);
            p.Image = await _pic.SaveFileAsync(p.image) ?? p.CoverImage;
            p.Image2 = await _pic.SaveFileAsync(p.image2) ?? p.CoverImage;
            p.Image3 = await _pic.SaveFileAsync(p.image3) ?? p.Image;

            var d = _entity.DestinationModelToDestiniton(p);
            d.DestinationID = 0;
            d.Turlider = u.Id;
            _Bll.Insert(d);
            TempData["success"] = $"Das Reiseziel „{d.City}“ wurde angelegt.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Bearbeiten
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var d = _Bll.GetById(id);
            if (d == null) return NotFound();
            var model = _entity.DestinitonToDestinationModel(d);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DestinationModel p)
        {
            ValidateImages(p);
            var dest = _Bll.GetById(p.DestinationID);
            if (dest == null) return NotFound();
            if (!ModelState.IsValid)
            {
                p.CoverImage = dest.CoverImage; p.Image = dest.Image; p.Image2 = dest.Image2; p.Image3 = dest.Image3;
                return View(p);
            }

            dest.City = p.City;
            dest.DayNight = p.DayNight;
            dest.Price = p.Price;
            dest.Capacity = p.Capacity;
            dest.Description = p.Description;
            dest.Status = p.Status;
            dest.Title1 = p.Title1; dest.Detail1 = p.Detail1; dest.Detail2 = p.Detail2;
            dest.Title3 = p.Title3; dest.Detail3 = p.Detail3;
            dest.Title4 = p.Title4; dest.Detail4 = p.Detail4;
            dest.Title5 = p.Title5; dest.Detail5 = p.Detail5;
            dest.CoverImage = await _pic.SaveFileAsync(p.coverImage) ?? dest.CoverImage;
            dest.Image = await _pic.SaveFileAsync(p.image) ?? dest.Image;
            dest.Image2 = await _pic.SaveFileAsync(p.image2) ?? dest.Image2;
            dest.Image3 = await _pic.SaveFileAsync(p.image3) ?? dest.Image3;
            _Bll.Update(dest);

            TempData["success"] = $"Das Reiseziel „{dest.City}“ wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Löschen / Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var d = _Bll.GetById(id);
            if (d != null)
            {
                _Bll.Delete(d);
                TempData["success"] = $"Das Reiseziel „{d.City}“ wurde gelöscht.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(int id, bool status)
        {
            var d = _Bll.GetById(id);
            if (d == null) return Json(new { success = false });
            _Bll.ToggleStatus(id, status);
            return Json(new { success = true, updated = status });
        }
        #endregion

        private void ValidateImages(DestinationModel p)
        {
            foreach (var (file, key) in new[] { (p.coverImage, nameof(p.coverImage)), (p.image, nameof(p.image)), (p.image2, nameof(p.image2)), (p.image3, nameof(p.image3)) })
                if (!PicSave.IsValidImage(file, out var err))
                    ModelState.AddModelError(key, err);
        }
    }
}
