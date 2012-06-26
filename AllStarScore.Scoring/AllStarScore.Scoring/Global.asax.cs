using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AllStarScore.Library.ModelBinding;
using AllStarScore.Library.Moth;
using AllStarScore.Scoring.Controllers;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Infrastructure.RavenQueryListeners;
using Moth.Core;
using Raven.Abstractions.Data;
using Raven.Client.Embedded;
using System.Diagnostics;
using Raven.Client.Indexes;
using RouteMagic;

namespace AllStarScore.Scoring
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CustomAuthorizeAttribute());
            filters.Add(new MothAction());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //http://haacked.com/archive/2011/02/02/redirecting-routes-to-maintain-persistent-urls.aspx
            var images = routes.MapRoute("content_images", "content/images/{file}");
            routes.Redirect(r => r.MapRoute("moth_images", "resources/images/{file}"))
                .To(images);

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: null,
                url: "summary/{performanceId}",
                defaults: new { controller = "Scoring", action = "Summary", performanceId = UrlParameter.Optional  }
            );

            routes.MapRoute(
                name: null,
                url: "entry/{performanceId}/judges-{judgeId}",
                defaults: new { controller = "Scoring", action = "ScoreEntry" }
            );

            routes.MapRoute(
                name: null,
                url: "performances/{id}",
                defaults: new { controller = "Performance", action = "Index" }
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

            InitializeMothForDebugging();
            MothRouteFactory.RegisterRoutes(RouteTable.Routes);

            RegisterRoutes(RouteTable.Routes);

            ValueProviderFactories.Factories.Insert(0, new CommandValueProviderFactory());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder()); //http://digitalbush.com/2011/04/24/asp-net-mvc3-json-decimal-binding-woes/

            //BundleTable.Bundles.RegisterTemplateBundles();

            //TOOD: Script netsh for installer: https://groups.google.com/forum/?fromgroups#!topic/ravendb/Wzl9jPq0m_0
            //http://blogs.iis.net/webdevelopertips/archive/2009/10/02/tip-98-did-you-know-the-default-application-pool-identity-in-iis-7-5-windows-7-changed-from-networkservice-to-apppoolidentity.aspx
            RavenController.DocumentStore = new EmbeddableDocumentStore()
                                            {
                                                ConnectionStringName = "RavenDB",
                                                UseEmbeddedHttpServer = true,
                                            };
            
            Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8085);
            RavenController.DocumentStore.Configuration.Port = 8085;
            RavenController.DocumentStore.RegisterListener(new NoStaleQueriesAllowedAsOfNow());
            RavenController.DocumentStore.Initialize();
            RavenController.DocumentStore.Conventions.IdentityPartsSeparator = "-";

            InitializeRavenProfiler();

            IndexCreation.CreateIndexes(typeof(JudgeScoreIndex).Assembly, RavenController.DocumentStore);
        }

        [Conditional("DEBUG")]
        private void InitializeMothForDebugging()
        {
            MothAction.Initialize(new CustomMothProvider());
        }

        [Conditional("DEBUG")]
        private void InitializeRavenProfiler()
        {
            Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(RavenController.DocumentStore);
        }
    }
}