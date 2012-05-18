using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AllStarScore.Admin.Controllers;
using AllStarScore.Admin.Models;
using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace AllStarScore.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
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
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.EnableDefaultBundles();//.RegisterTemplateBundles();

            var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            parser.Parse();

            RavenController.DocumentStore = new DocumentStore()
            {
                ApiKey = parser.ConnectionStringOptions.ApiKey,
                Url = parser.ConnectionStringOptions.Url,
            }.Initialize();

            Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(RavenController.DocumentStore);
        
            //RavenController.DocumentStore.Conventions.IdentityPartsSeparator = "-";
            HackSecurity();
        }

        //TODO: come up with something better and remove this
        private void HackSecurity()
        {
            var session = RavenController.DocumentStore.OpenSession();
            if (!session.Query<User>().Any())
            {
                var admin = new User();
                admin.Email = "admin@wyldeye.com";
                admin.UserName = "administrator";
                admin.Enabled = true;
                admin.SetPassword("hello");

                session.Store(admin);
                session.SaveChanges();
            }            
        }
    }
}