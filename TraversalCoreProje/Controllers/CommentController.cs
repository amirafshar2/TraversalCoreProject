using BusinessLayer.Abstract;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Controllers
{
    /// <summary>Kommentare auf der Detailseite eines Reiseziels (nur für angemeldete Benutzer).</summary>
    public class CommentController : Controller
    {
        private readonly ICommentService _comment;
        private readonly UserManager<User> _usermanager;

        public CommentController(ICommentService comment, UserManager<User> usermanager)
        {
            _comment = comment;
            _usermanager = usermanager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment([FromBody] mComment c)
        {
            var user = await _usermanager.GetUserAsync(HttpContext.User);
            if (user == null)
                return Unauthorized(new { success = false, message = "Bitte melden Sie sich an." });

            var content = (c?.CommentContent ?? "").Trim();
            if (content.Length < 3 || content.Length > 1000)
                return BadRequest(new { success = false, message = "Der Kommentar muss zwischen 3 und 1000 Zeichen lang sein." });

            var comment = new Comment
            {
                CommentData = DateTime.Now,
                status = true,
                CommentContent = content,
                CommentUser = $"{user.Name} {user.Surname}",
                Destinitonid = c.DestinationId,
                Userid = user.Id
            };
            _comment.Insert(comment);

            return Json(new
            {
                success = true,
                comment = new
                {
                    name = comment.CommentUser,
                    image = user.Image,
                    content = comment.CommentContent,
                    date = comment.CommentData.ToString("dd.MM.yyyy HH:mm")
                }
            });
        }
    }
}
