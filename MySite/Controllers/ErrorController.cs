using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySite.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index(string message = "Error", string returnUrl = "~/Error/Index")
        {
            ViewBag.message = message;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        public ActionResult HttpError404(string message = "Error", string returnUrl = "~/Error/Index")
        {
            ViewBag.message = message;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        public ActionResult HttpError500(string message = "Error", string returnUrl = "~/Error/Index")
        {
            ViewBag.message = message;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        public ActionResult General(string message = "Error", string returnUrl = "~/Error/Index")
        {
            ViewBag.message = message;
            ViewBag.returnUrl = returnUrl;
            return View();
        }
    }
}


