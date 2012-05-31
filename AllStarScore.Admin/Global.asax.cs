using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AllStarScore.Admin.Controllers;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Infrastructure.ModelBinding;
using AllStarScore.Admin.Models;
using Moth.Core;
using Moth.Core.Providers;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using RouteMagic;

namespace AllStarScore.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
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
                url: "gym/search/{query}",
                defaults: new { controller = "Gym", action = "Search" }
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

            InitializeMothForDebugging();
            MothRouteFactory.RegisterRoutes(RouteTable.Routes);
            
            RegisterRoutes(RouteTable.Routes);

            ValueProviderFactories.Factories.Insert(0, new CommandValueProviderFactory()); //TODO: Blog

            //BundleTable.Bundles.EnableDefaultBundles();//.RegisterTemplateBundles();

            var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            parser.Parse();

            RavenController.DocumentStore = new DocumentStore()
            {
                ApiKey = parser.ConnectionStringOptions.ApiKey,
                Url = parser.ConnectionStringOptions.Url,
            };
            RavenController.DocumentStore.Initialize();
            RavenController.DocumentStore.Conventions.IdentityPartsSeparator = "-";

            Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(RavenController.DocumentStore);

            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(GymsByName).Assembly, RavenController.DocumentStore);
            
            HackSecurity();
            HackLevels();
            HackDivisions();
        }

        [Conditional("DEBUG")]
        private void InitializeMothForDebugging()
        {
            MothAction.Initialize(new CustomMothProvider());
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

        //TODO: come up with something better and remove this
        private void HackLevels()
        {
            var session = RavenController.DocumentStore.OpenSession();
            var ok = session.Query<Level>().Any();
            if (ok) return;

            var levels = new List<Level>()
                             {
                                 new Level {Name = "Level 1"},
                                 new Level {Name = "Level 2"},
                                 new Level {Name = "Level 3"},
                                 new Level {Name = "Level 4"},
                                 new Level {Name = "Level 5"},
                                 new Level {Name = "Level 6"},
                                 new Level {Name = "Dance"},
                                 new Level {Name = "School"},
                                 new Level {Name = "Individual"}
                             };
            
            levels.ForEach(session.Store);
            session.SaveChanges();
        }

        //TODO: come up with something better and remove this
        private void HackDivisions()
        {
            var session = RavenController.DocumentStore.OpenSession();
            var ok = session.Query<Division>().Any();
            if (ok) return;

            var levels = session.Query<Level>().ToList();

            var divisions = new List<Division>()
                             {
                                 new Division{ Name = "Small Youth", LevelId = levels[0].Id},
                                 new Division{ Name = "Large Youth", LevelId = levels[1].Id},
                                 new Division{ Name = "Small Junior", LevelId = levels[2].Id},
                                 new Division{ Name = "Large Junior", LevelId = levels[3].Id},
                                 new Division{ Name = "Small Senior", LevelId = levels[4].Id},
                                 new Division{ Name = "Large Senior", LevelId = levels[5].Id}
                             };

            divisions.ForEach(session.Store);
            session.SaveChanges();
        }
    }

    public class CustomMothProvider : AspNetCacheProvider
    {
        public override void Store(string key, object o, TimeSpan duration)
        {
            /* tah dah */
        }

        public override IOutputCacheRestrictions Enable
        {
            get
            {
                return new OutputCacheRestrictions()
                {
                    PageOutput = true, 
                    CssTidy = false,   
                    ScriptMinification = false,
                    CssPreprocessing = false 
                };
            }
        }
    }

    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!(filterContext.Controller is ResourcesController))
                base.OnAuthorization(filterContext);
        }
    }
}