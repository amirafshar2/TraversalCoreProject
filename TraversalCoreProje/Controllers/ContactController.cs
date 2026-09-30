using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        private readonly IContactServic _contact;
        private readonly IContactMessageService _message;

        public ContactController(IContactServic contact, IContactMessageService message)
        {
            _contact = contact;
            _message = message;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactPageViewModel { Contact = _contact.GetAll().FirstOrDefault() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactPageViewModel model)
        {
            var msg = model.Message ?? new ContactMessage();
            var result = new ContactMessageValidator().Validate(msg);
            if (!result.IsValid)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("Message." + e.PropertyName, e.ErrorMessage);
                model.Contact = _contact.GetAll().FirstOrDefault();
                return View(model);
            }

            msg.MessageDate = DateTime.Now;
            msg.IsRead = false;
            _message.Insert(msg);
            TempData["success"] = "Vielen Dank! Ihre Nachricht wurde gesendet. Wir melden uns so schnell wie möglich.";
            return RedirectToAction(nameof(Index));
        }
    }
}
