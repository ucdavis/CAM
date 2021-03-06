﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CAM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "request",
                url: "{site}/request/{id}",
                defaults: new { site = "none", controller = "Request", action = "Index", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
                );

            routes.MapRoute(
                name: "acct",
                url: "Account/{action}",
                defaults: new { site = "", controller = "Account", action = "Index" }
                );

            routes.MapRoute(
                name: "sitebase",
                url: "{site}/{controller}/{action}/{id}",
                defaults: new {site = "none", controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();

            AutomapperConfig.Configure();
        }
    }
}