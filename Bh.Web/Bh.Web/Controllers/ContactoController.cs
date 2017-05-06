using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bh.Web.Services;
using Bh.Web.ViewModels;

namespace Bh.Web.Controllers
{
    public class ContactoController : Controller
    {
        // GET: Contacto
        public ActionResult Index()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public ActionResult Index(ContactViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (viewmodel.CaptchaIsValid())
                {
                    var emailService = new EmailService();
                    emailService.SendContactMessage(viewmodel);
                }
                return RedirectToAction("MensajeEnviado");
            }

            return View(viewmodel);

        }

        public ActionResult MensajeEnviado()
        {
            return View();
        }
    }
}