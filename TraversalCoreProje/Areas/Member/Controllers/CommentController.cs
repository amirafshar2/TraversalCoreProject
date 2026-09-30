using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace TraversalCoreProje.Areas.Member.Controllers
{
    /// <summary>Eigene Kommentare des Kunden anzeigen und löschen.</summary>
    [Area("Member")]
    public class CommentController : Controller
    {
        private readonly ICommentService _comment;
        private readonly UserManager<User> _userManager;

        public CommentController(ICommentService comment, UserManager<User> userManager)
        {
            _comment = comment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "User", new { area = "" });
            ViewBag.UserImage = user.Image;
            return View(_comment.GetCommentsByUserID(user.Id).ToPagedList(page, 6));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var c = _comment.GetById(id);
            if (c != null && user != null && c.Userid == user.Id)
            {
                _comment.Delete(c);
                TempData["success"] = "Ihr Kommentar wurde gelöscht.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
