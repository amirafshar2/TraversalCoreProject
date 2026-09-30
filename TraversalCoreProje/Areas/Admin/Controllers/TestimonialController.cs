using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models.PicMethods;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Kundenstimmen für die Startseite verwalten.</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TestimonialController : Controller
    {
        private readonly ITestimonialServic _testimonial;
        private readonly PicSave _pic = new();

        public TestimonialController(ITestimonialServic testimonial)
        {
            _testimonial = testimonial;
        }

        public IActionResult Index() => View(_testimonial.GetAll());

        [HttpGet]
        public IActionResult Create() => View("Form", new Testimonial { Status = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial model, IFormFile imageFile)
        {
            if (!Validate(model, imageFile)) return View("Form", model);
            model.ClientImage = await _pic.SaveFileAsync(imageFile) ?? "/Traversal-Starter/assets/images/c1.jpg";
            _testimonial.Insert(model);
            TempData["success"] = "Die Kundenstimme wurde angelegt.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var t = _testimonial.GetById(id);
            return t == null ? NotFound() : View("Form", t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Testimonial model, IFormFile imageFile)
        {
            var t = _testimonial.GetById(model.TestimonialID);
            if (t == null) return NotFound();
            if (!Validate(model, imageFile)) { model.ClientImage = t.ClientImage; return View("Form", model); }
            t.Client = model.Client;
            t.Location = model.Location;
            t.Comment = model.Comment;
            t.Status = model.Status;
            t.ClientImage = await _pic.SaveFileAsync(imageFile) ?? t.ClientImage;
            _testimonial.Update(t);
            TempData["success"] = "Die Kundenstimme wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var t = _testimonial.GetById(id);
            if (t != null) { _testimonial.Delete(t); TempData["success"] = "Die Kundenstimme wurde gelöscht."; }
            return RedirectToAction(nameof(Index));
        }

        private bool Validate(Testimonial model, IFormFile imageFile)
        {
            if (string.IsNullOrWhiteSpace(model.Client)) ModelState.AddModelError(nameof(model.Client), "Bitte geben Sie einen Namen ein.");
            if (string.IsNullOrWhiteSpace(model.Comment) || model.Comment.Length < 10) ModelState.AddModelError(nameof(model.Comment), "Der Text muss mindestens 10 Zeichen lang sein.");
            if (!PicSave.IsValidImage(imageFile, out var err)) ModelState.AddModelError("imageFile", err);
            return ModelState.IsValid;
        }
    }
}
