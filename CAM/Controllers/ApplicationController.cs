using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using UCDArch.Web.Controller;

namespace CAM.Controllers
{
    public class ApplicationController : SuperController
    {
        public string Site { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Site = filterContext.RouteData.Values["site"] as string;
            base.OnActionExecuting(filterContext);
        }

        public Site LoadSite()
        {
            return Repository.OfType<Site>().Queryable.FirstOrDefault(a => a.Id == Site);
        }
        
    }
}
