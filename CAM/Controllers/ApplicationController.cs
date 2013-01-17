using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using UCDArch.Web.Controller;

namespace CAM.Controllers
{
    [Authorize]
    public class ApplicationController : SuperController
    {
        public string Site { get; private set; }
        protected string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Site = filterContext.RouteData.Values["site"] as string;
            base.OnActionExecuting(filterContext);
        }

        public Site LoadSite()
        {
            return Repository.OfType<Site>().Queryable.FirstOrDefault(a => a.Id == Site);
        }

        private const string MessageKey = "Message";
        public string Message { 
            get { return TempData[MessageKey] as string; }
            set { TempData[MessageKey] = value; }
        }
    }
}
