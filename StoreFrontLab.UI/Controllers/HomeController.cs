using System.Web.Mvc;

namespace StoreFrontLab.UI.Controllers
{
    public class HomeController : Controller
    {
        //[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            

            return View();
        }

        //[HttpGet]
        public ActionResult Contact()
        {
            

            return View();
        }
    }
}
