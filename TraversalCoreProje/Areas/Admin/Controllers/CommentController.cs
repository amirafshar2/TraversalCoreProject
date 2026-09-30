using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Areas.Admin.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    /// <summary>Kommentare prüfen, freischalten/sperren und löschen (Admin und Moderator).</summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class CommentController : Controller
    {
        private readonly ICommentService _Bll;
        private readonly IUserService _userService;

        public CommentController(ICommentService bll, IUserService userService)
        {
            _Bll = bll;
            _userService = userService;
        }

        public IActionResult Index(string filter, int page = 1)
        {
            var users = _userService.GetAll().ToDictionary(u => u.Id);
            var comments = _Bll.GetAllForAdmin().AsEnumerable();
            if (filter == "active") comments = comments.Where(c => c.status);
            if (filter == "inactive") comments = comments.Where(c => !c.status);

            var list = comments.Select(item =>
            {
                users.TryGetValue(item.Userid, out var user);
                return new CommentWithDestinationundUserModel
                {
                    id = item.id,
                    CommentContent = item.CommentContent,
                    CommentData = item.CommentData,
                    CommentUser = item.CommentUser,
                    DestinationID = item.Destinitonid,
                    DesCity = item.Destiniton?.City,
                    DesImage = item.Destiniton?.Image,
                    status = item.status,
                    UserID = item.Userid,
                    UserImage = user?.Image,
                    UserName = user?.Name ?? item.CommentUser,
                    UserSurname = user?.Surname
                };
            }).ToList();

            ViewBag.Filter = filter;
            ViewBag.CountAll = _Bll.GetAllForAdmin().Count;
            ViewBag.CountInactive = _Bll.GetAllForAdmin().Count(c => !c.status);
            return View(list.ToPagedList(page, 8));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int id, string returnUrl)
        {
            _Bll.ToggleStatus(id);
            TempData["success"] = "Der Status des Kommentars wurde geändert.";
            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delate(int id, string returnUrl)
        {
            var c = _Bll.GetById(id);
            if (c != null)
            {
                _Bll.Delete(c);
                TempData["success"] = "Der Kommentar wurde gelöscht.";
            }
            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction(nameof(Index));
        }
    }
}
