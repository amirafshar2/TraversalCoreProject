using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models.PicMethods;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Angebots-Kacheln („Top-Angebote“) der Startseite verwalten.</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FeatureController : Controller
    {
        private readonly IFeatureServic _feature;
        private readonly PicSave _pic = new();

        public FeatureController(IFeatureServic feature)
        {
            _feature = feature;
        }

        public IActionResult Index() => View(_feature.GetAll());

        [HttpGet]
        public IActionResult Create() => View("Form", new Feature { Status = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature model, IFormFile imageFile)
        {
            if (imageFile == null) ModelState.AddModelError("imageFile", "Bitte laden Sie ein Bild hoch.");
            if (!Validate(model, imageFile)) return View("Form", model);
            model.Image = await _pic.SaveFileAsync(imageFile);
            _feature.Insert(model);
            TempData["success"] = "Das Angebot wurde angelegt.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var f = _feature.GetById(id);
            return f == null ? NotFound() : View("Form", f);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Feature model, IFormFile imageFile)
        {
            var f = _feature.GetById(model.FeatureId);
            if (f == null) return NotFound();
            if (!Validate(model, imageFile)) { model.Image = f.Image; return View("Form", model); }
            f.Title = model.Title;
            f.Description = model.Description;
            f.Status = model.Status;
            f.Image = await _pic.SaveFileAsync(imageFile) ?? f.Image;
            _feature.Update(f);
            TempData["success"] = "Das Angebot wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var f = _feature.GetById(id);
            if (f != null) { _feature.Delete(f); TempData["success"] = "Das Angebot wurde gelöscht."; }
            return RedirectToAction(nameof(Index));
        }

        private bool Validate(Feature model, IFormFile imageFile)
        {
            if (string.IsNullOrWhiteSpace(model.Title)) ModelState.AddModelError(nameof(model.Title), "Bitte geben Sie einen Titel ein.");
            if (string.IsNullOrWhiteSpace(model.Description)) ModelState.AddModelError(nameof(model.Description), "Bitte geben Sie einen Preis/Text ein.");
            if (!PicSave.IsValidImage(imageFile, out var err)) ModelState.AddModelError("imageFile", err);
            return ModelState.IsValid;
        }
    }
}
