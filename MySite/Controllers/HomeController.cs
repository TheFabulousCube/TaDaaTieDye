using System.Web.Mvc;

namespace MySite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome To Ta Daa Tie Dye!";

            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "All about me . . .";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Our contact page.";

            return View();
        }

        public ActionResult Error()
        {

            return View();
        }
    }
}
