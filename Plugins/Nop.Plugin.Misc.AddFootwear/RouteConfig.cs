using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nop.Web.Framework.Mvc.Routes;
using System.Web.Routing;
using Nop.Plugin.Misc.AddFootwear.Infrastructure;


namespace Nop.Plugin.Misc.AddFootwear
{
    public class RouteConfig : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Misc.AddFootwear.ManageFootwear",
                "Footwear/Manage",
                new {Controller = "Footwear", Action = "Manage"},
                new[] { "Nop.Plugin.Misc.AddFootwear.Controllers" }
                );
            ViewEngines.Engines.Insert(0,new CustomViewEngine());
        }

        public int Priority => 0;
    }
}
