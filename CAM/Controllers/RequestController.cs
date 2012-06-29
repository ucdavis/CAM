using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CAM.Core.Domain;

namespace CAM.Controllers
{
    public class RequestController : Controller
    {
        public ActionResult Index()
        {
            var request = new Request();
            return View(request);
        }

    }
}
