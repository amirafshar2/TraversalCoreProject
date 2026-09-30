using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models.PicMethods;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Seiteninhalte: „Über uns“, Startseiten-Banner, Vorteile und Kontaktdaten.</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContentController : Controller
    {
        private readonly IAboutServic _about;
        private readonly IAbout2Service _about2;
        private readonly ISubAboutServic _subAbout;
        private readonly IContactServic _contact;
        private readonly PicSave _pic = new();

        public ContentController(IAboutServic about, IAbout2Service about2, ISubAboutServic subAbout, IContactServic contact)
        {
            _about = about;
            _about2 = about2;
            _subAbout = subAbout;
            _contact = contact;
        }

        public IActionResult Index()
        {
            ViewBag.About = _about.GetAll().FirstOrDefault() ?? new About { Status = true };
            ViewBag.About2 = _about2.GetAll().FirstOrDefault() ?? new About2();
            ViewBag.SubAbouts = _subAbout.GetAll();
            ViewBag.Contact = _contact.GetAll().FirstOrDefault() ?? new Contact { Status = true };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAbout(About model, IFormFile imageFile)
        {
            var result = new AboutValidator().Validate(model);
            if (!result.IsValid || !PicSave.IsValidImage(imageFile, out _))
            {
                TempData["error"] = string.Join(" ", result.Errors.Select(e => e.ErrorMessage).DefaultIfEmpty("Ungültiges Bild."));
                return RedirectToAction(nameof(Index));
            }
            var a = _about.GetById(model.AboutID);
            if (a == null)
            {
                model.Image1 = await _pic.SaveFileAsync(imageFile) ?? model.Image1;
                model.Status = true;
                _about.Insert(model);
            }
            else
            {
                a.Title = model.Title; a.Description = model.Description;
                a.Title2 = model.Title2; a.Description2 = model.Description2;
                a.Image1 = await _pic.SaveFileAsync(imageFile) ?? a.Image1;
                _about.Update(a);
            }
            TempData["success"] = "Die Seite „Über uns“ wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveBanner(About2 model, IFormFile imageFile)
        {
            if (string.IsNullOrWhiteSpace(model.Title1) || string.IsNullOrWhiteSpace(model.Descriotion) || !PicSave.IsValidImage(imageFile, out _))
            {
                TempData["error"] = "Bitte Titel und Text ausfüllen (und nur Bilddateien hochladen).";
                return RedirectToAction(nameof(Index));
            }
            var a = _about2.GetById(model.Aboute2ID);
            if (a == null)
            {
                model.Image = await _pic.SaveFileAsync(imageFile) ?? "/Traversal-Liberty/assets/images/banner2.jpg";
                _about2.Insert(model);
            }
            else
            {
                a.Title1 = model.Title1; a.Title2 = model.Title2; a.Descriotion = model.Descriotion;
                a.Image = await _pic.SaveFileAsync(imageFile) ?? a.Image;
                _about2.Update(a);
            }
            TempData["success"] = "Der Startseiten-Banner wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveSubAbout(SubAbout model)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
            {
                TempData["error"] = "Bitte Titel und Beschreibung ausfüllen.";
                return RedirectToAction(nameof(Index));
            }
            if (model.SubAboutID == 0) _subAbout.Insert(model);
            else
            {
                var s = _subAbout.GetById(model.SubAboutID);
                if (s == null) return NotFound();
                s.Title = model.Title; s.Description = model.Description;
                _subAbout.Update(s);
            }
            TempData["success"] = "Der Vorteil wurde gespeichert.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSubAbout(int id)
        {
            var s = _subAbout.GetById(id);
            if (s != null) _subAbout.Delete(s);
            TempData["success"] = "Der Vorteil wurde gelöscht.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveContact(Contact model)
        {
            if (string.IsNullOrWhiteSpace(model.Mail) || string.IsNullOrWhiteSpace(model.Adress))
            {
                TempData["error"] = "Bitte mindestens E-Mail und Adresse angeben.";
                return RedirectToAction(nameof(Index));
            }
            var c = _contact.GetById(model.ContactID);
            if (c == null) { model.Status = true; _contact.Insert(model); }
            else
            {
                c.Description = model.Description; c.Mail = model.Mail; c.Adress = model.Adress;
                c.Phone = model.Phone; c.MapLocation = model.MapLocation;
                _contact.Update(c);
            }
            TempData["success"] = "Die Kontaktdaten wurden gespeichert.";
            return RedirectToAction(nameof(Index));
        }
    }
}
