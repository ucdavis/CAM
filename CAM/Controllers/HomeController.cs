using System.Web.Mvc;
using CAM.Core.Domain;

namespace CAM.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            var sites = Repository.OfType<Site>().GetAll();

            return View(sites);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }
    }
}
