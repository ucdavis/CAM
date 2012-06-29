using System.Web.Mvc;
using CAM.Core.Domain;

namespace CAM.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var test = Repository.OfType<RequestTemplate>().GetAll();

            return View();
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
