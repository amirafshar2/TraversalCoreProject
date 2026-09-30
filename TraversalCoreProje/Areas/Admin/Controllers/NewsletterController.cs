using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Newsletter-Abonnenten anzeigen, exportieren und löschen.</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsletterController : Controller
    {
        private readonly INewsletterService _newsletter;

        public NewsletterController(INewsletterService newsletter)
        {
            _newsletter = newsletter;
        }

        public IActionResult Index() => View(_newsletter.GetAll().OrderByDescending(n => n.SubscribeDate).ToList());

        public IActionResult Export()
        {
            var sb = new StringBuilder("E-Mail;Angemeldet am\n");
            foreach (var n in _newsletter.GetAll().OrderBy(n => n.Mail))
                sb.AppendLine($"{n.Mail};{n.SubscribeDate:dd.MM.yyyy}");
            return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray(),
                "text/csv", "newsletter.csv");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var n = _newsletter.GetById(id);
            if (n != null) { _newsletter.Delete(n); TempData["success"] = $"{n.Mail} wurde entfernt."; }
            return RedirectToAction(nameof(Index));
        }
    }
}
