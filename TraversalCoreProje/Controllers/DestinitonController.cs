using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class DestinitonController : Controller
    {
        private readonly IDestinitionServic _destination;
        private readonly IGuideService _guide;
        private readonly ICommentService _comment;

        public DestinitonController(IDestinitionServic destination, IGuideService guide, ICommentService comment)
        {
            _destination = destination;
            _guide = guide;
            _comment = comment;
        }

        /// <summary>Liste aller aktiven Reiseziele mit Suche und Sortierung.</summary>
        public IActionResult Index(string q, string sort)
        {
            var list = _destination.GetActive().AsEnumerable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                list = list.Where(d => (d.City ?? "").Contains(term, StringComparison.OrdinalIgnoreCase)
                                    || (d.Description ?? "").Contains(term, StringComparison.OrdinalIgnoreCase));
            }
            list = sort switch
            {
                "price-asc" => list.OrderBy(d => d.Price),
                "price-desc" => list.OrderByDescending(d => d.Price),
                "name" => list.OrderBy(d => d.City),
                _ => list.OrderByDescending(d => d.DestinationID)
            };
            return View(new DestinationListViewModel { Destinations = list.ToList(), Search = q, Sort = sort });
        }

        /// <summary>Detailseite eines Reiseziels.</summary>
        [HttpGet]
        public IActionResult Destiniton(int id)
        {
            var d = _destination.GetById(id);
            if (d == null || !d.Status) return NotFound();

            var guides = _guide.GetActive();
            var model = new DestinationDetailViewModel
            {
                Destination = d,
                Guide = guides.Count > 0 ? guides[id % guides.Count] : null,
                CommentCount = _comment.GetCommentsByDestinionID(id).Count,
                Related = _destination.GetActive().Where(x => x.DestinationID != id)
                                      .OrderBy(x => Guid.NewGuid()).Take(3).ToList()
            };
            return View(model);
        }
    }
}
