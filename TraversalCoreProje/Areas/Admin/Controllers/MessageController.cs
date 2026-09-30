using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Nachrichten aus dem Kontaktformular.</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MessageController : Controller
    {
        private readonly IContactMessageService _message;

        public MessageController(IContactMessageService message)
        {
            _message = message;
        }

        public IActionResult Index(int page = 1)
        {
            return View(_message.GetAll().OrderByDescending(m => m.MessageDate).ToPagedList(page, 10));
        }

        public IActionResult Details(int id)
        {
            var m = _message.GetById(id);
            if (m == null) return NotFound();
            _message.MarkAsRead(id);
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var m = _message.GetById(id);
            if (m != null) { _message.Delete(m); TempData["success"] = "Die Nachricht wurde gelöscht."; }
            return RedirectToAction(nameof(Index));
        }
    }
}
