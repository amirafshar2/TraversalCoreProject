using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.Panel
{
    /// <summary>Ungelesene Kontaktanfragen in der Navigationsleiste (nur Admin).</summary>
    public class _MessageDropdown : ViewComponent
    {
        private readonly IContactMessageService _message;

        public _MessageDropdown(IContactMessageService message)
        {
            _message = message;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Unread = _message.CountUnread();
            return View(_message.GetLatest(5));
        }
    }
}
