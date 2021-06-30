using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DiecastDecals
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "singlepro",
            url: "ProductNewUI/Index/{id}",
            defaults: new { controller = "ProductNewUI", action = "Index", id = UrlParameter.Optional}
            );

            routes.MapRoute(
            name: "blogui",
            url: "BlogUI/Index/{id}/{desc}/{catid}",
            defaults: new { controller = "BlogUI", action = "Index", id = UrlParameter.Optional, desc = UrlParameter.Optional,catid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "IndexUI", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
