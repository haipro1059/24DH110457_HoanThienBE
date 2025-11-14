using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _24DH110457_MyStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                // THÊM THAM SỐ NAMESPACES ĐỂ CHỈ ĐỊNH CONTROLLER GỐC
                namespaces: new[] { "_24DH110457_MyStore.Controllers" }
            );
        }
    }
}