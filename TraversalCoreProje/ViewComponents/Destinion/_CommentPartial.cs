using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Areas.Admin.Models;

namespace TraversalCoreProje.ViewComponents.Destinion
{
    /// <summary>Kommentarliste + Formular auf der Detailseite eines Reiseziels.</summary>
    public class _CommentPartial : ViewComponent
    {
        private readonly ICommentService _Bll;
        private readonly IUserService _user;
        private readonly UserManager<EntityLayer.Concrate.User> _usermanager;

        public _CommentPartial(ICommentService bll, IUserService user, UserManager<EntityLayer.Concrate.User> usermanager)
        {
            _Bll = bll;
            _user = user;
            _usermanager = usermanager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var currentUser = User.Identity?.IsAuthenticated == true ? await _usermanager.GetUserAsync(UserClaimsPrincipal) : null;
            ViewData["ProfileImage"] = currentUser?.Image ?? "/otika-bootstrap-admin-template/assets/img/users/user-2.png";
            ViewData["CurrentUser"] = currentUser != null ? $"{currentUser.Name} {currentUser.Surname}" : null;
            ViewData["desid"] = id;

            var comments = _Bll.GetCommentsByDestinionID(id).OrderByDescending(x => x.CommentData).ToList();
            var users = _user.GetAll().ToDictionary(u => u.Id);
            var models = comments.Select(item =>
            {
                users.TryGetValue(item.Userid, out var u);
                return new CommentWhitUserModel
                {
                    CommentContent = item.CommentContent,
                    CommentData = item.CommentData,
                    CommentUser = item.CommentUser,
                    Destinitonid = item.Destinitonid,
                    id = item.id,
                    status = item.status,
                    Userid = item.Userid,
                    UserImage = u?.Image,
                    UserName = u?.Name ?? item.CommentUser,
                    UserSurname = u?.Surname
                };
            }).ToList();
            return View(models);
        }
    }
}
