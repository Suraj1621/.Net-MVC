using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Register
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


           

            routes.MapRoute(
          name: "DeleteEmployee",
          url: "Delete/Employee/{id}",
          defaults: new { controller = "Employee", action = "Delete", id = UrlParameter.Optional }
           );





            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Employee", action = "Login", id = UrlParameter.Optional }
            );

           

        }
    }
}
