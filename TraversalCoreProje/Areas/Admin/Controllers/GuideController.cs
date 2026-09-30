using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models.PicMethods;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Reiseführer verwalten (CRUD) – Validierung mit FluentValidation.</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GuideController : Controller
    {
        private readonly IGuideService _guide;
        private readonly IReservationService _reservation;
        private readonly PicSave _pic = new();

        public GuideController(IGuideService guide, IReservationService reservation)
        {
            _guide = guide;
            _reservation = reservation;
        }

        public IActionResult Index()
        {
            var res = _reservation.GetAll();
            ViewBag.TourCounts = res.Where(r => r.Guidid.HasValue && r.status != ReservationStatus.Canceled)
                                    .GroupBy(r => r.Guidid.Value).ToDictionary(g => g.Key, g => g.Count());
            return View(_guide.GetAll());
        }

        [HttpGet]
        public IActionResult Create() => View("Form", new Guide { Status = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guide model, IFormFile imageFile)
        {
            if (!Validate(model, imageFile)) return View("Form", model);
            model.Image = await _pic.SaveFileAsync(imageFile) ?? "/Traversal-Starter/assets/images/team1.jpg";
            _guide.Insert(model);
            TempData["success"] = $"Der Reiseführer {model.Name} wurde angelegt.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var g = _guide.GetById(id);
            return g == null ? NotFound() : View("Form", g);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guide model, IFormFile imageFile)
        {
            var g = _guide.GetById(model.GuideID);
            if (g == null) return NotFound();
            if (!Validate(model, imageFile)) { model.Image = g.Image; return View("Form", model); }

            g.Name = model.Name;
            g.Description = model.Description;
            g.TwitterUrl = model.TwitterUrl;
            g.InstagramUrl = model.InstagramUrl;
            g.Status = model.Status;
            g.Image = await _pic.SaveFileAsync(imageFile) ?? g.Image;
            _guide.Update(g);
            TempData["success"] = $"Der Reiseführer {g.Name} wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int id)
        {
            var g = _guide.GetById(id);
            if (g != null) { g.Status = !g.Status; _guide.Update(g); }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var g = _guide.GetById(id);
            if (g != null) { _guide.Delete(g); TempData["success"] = $"Der Reiseführer {g.Name} wurde gelöscht."; }
            return RedirectToAction(nameof(Index));
        }

        private bool Validate(Guide model, IFormFile imageFile)
        {
            var result = new GuideValidator().Validate(model);
            foreach (var e in result.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            if (!PicSave.IsValidImage(imageFile, out var err)) ModelState.AddModelError("imageFile", err);
            // Nur http(s)-Links zulassen (verhindert z. B. "javascript:"-Links)
            model.TwitterUrl = SafeUrl(model.TwitterUrl);
            model.InstagramUrl = SafeUrl(model.InstagramUrl);
            return ModelState.IsValid;
        }

        private static string SafeUrl(string url) =>
            !string.IsNullOrWhiteSpace(url) && Uri.TryCreate(url.Trim(), UriKind.Absolute, out var u)
            && (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps) ? u.ToString() : "#";
    }
}
