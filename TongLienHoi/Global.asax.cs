using HTTLVN.QLTLH.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace HTTLVN.QLTLH
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
               name: "document",
               url: "document/{id}/{sheet}",
               defaults: new { controller = "Home", action = "Document", id = UrlParameter.Optional },
               namespaces: new string[] { "HTTLVN.QLTLH.Controllers" }
               );
            routes.MapRoute(
              name: "DocumentAssignment",
              url: "DocumentAssignment/{id}/{sheet}",
              defaults: new { controller = "Home", action = "DocumentAssignment", id = UrlParameter.Optional },
              namespaces: new string[] { "HTTLVN.QLTLH.Controllers" }
              );
            routes.MapRoute(
              name: "Assignment",
              url: "Assignment/{id}/{sheet}",
              defaults: new { controller = "Home", action = "Assignment", id = UrlParameter.Optional },
              namespaces: new string[] { "HTTLVN.QLTLH.Controllers" }
              );
            //routes.MapRoute(
            // name: "vanthuden",
            // url: "van-thu-den",
            // defaults: new { controller = "VanThu", action = "Index" ,id = 1 }
            // );
            //routes.MapRoute(
            // name: "vanthudi",
            // url: "van-thu-di",
            // defaults: new { controller = "VanThu", action = "Index", id = 2 }
            // );
            //routes.MapRoute(
            // name: "vanthuden",
            // url: "VanThuDen",
            // defaults: new { controller = "Home", action = "VanThu", id = UrlParameter.Optional},
            // namespaces: new string[] { "HTTLVN.QLTLH.Controllers" }
            // );
            //routes.MapRoute(
            // name: "vanthudi",
            // url: "VanThuDi",
            // defaults: new { controller = "Home", action = "VanThu", id = 2 },
            // namespaces: new string[] { "HTTLVN.QLTLH.Controllers" }
            // );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new string[] { "HTTLVN.QLTLH.Controllers" }
                );
            
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}