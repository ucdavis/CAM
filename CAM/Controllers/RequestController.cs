using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CAM.Core.Domain;
using UCDArch.Web.ActionResults;

namespace CAM.Controllers
{
    public class RequestController : Controller
    {
        public ActionResult Index(string id)
        {
            var request = new Request();
            return View(request);
        }

        public JsonNetResult LoadRoleTemplate(string role)
        {
            throw new NotImplementedException();
        }

    }
}
