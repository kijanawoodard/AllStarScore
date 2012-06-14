using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AllStarScore.Scoring.Controllers;
using Raven.Client.Embedded;

namespace AllStarScore.Scoring
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

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Landing", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();

            RavenController.DocumentStore = new EmbeddableDocumentStore()
                                            {
                                                DataDirectory = "Raven",
                                                UseEmbeddedHttpServer = true
                                            };
            Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8085);
            RavenController.DocumentStore.Configuration.Port = 8085;
            RavenController.DocumentStore.Initialize();
            RavenController.DocumentStore.Conventions.IdentityPartsSeparator = "-";

            
        }
        protected void Application_End()
        {
            RavenController.DocumentStore.Dispose();
            Thread.Sleep(5);
        }
    }
}