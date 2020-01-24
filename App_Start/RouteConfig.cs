using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IMDeanyP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Add Actor (Film)",
                url: "Actings/Create/{subName}/{FilmId}",
                defaults: new { controller = "Actingd", Action = "Create", FilmId = UrlParameter.Optional},
                //as long as this subname is given, the above route applies
                constraints: new { subName = "Film" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional } 
            );
        }
    }
}
