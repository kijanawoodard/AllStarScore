using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AllStarScore.Admin.Controllers;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;
using AllStarScore.Library.ModelBinding;
using AllStarScore.Library.Moth;
using AllStarScore.Models;
using Moth.Core;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
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
                url: "synchronize/{hash}/{competitionid}",
                defaults: new { controller = "Sync", action = "Export" }
            );

            routes.MapRoute(
                name: null,
                url: "gym/search/{query}",
                defaults: new { controller = "Gym", action = "Search" }
            );

            routes.MapRoute(
                name: null,
                url: "registration/{competitionid}/{gymid}",
                defaults: new { controller = "Registration", action = "Index" }
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

            InitializeRavenProfiler();

            IndexCreation.CreateIndexes(typeof (GymsByName).Assembly, RavenController.DocumentStore);
//            CreateIndexesForDatabases(typeof (GymsByName).Assembly, RavenController.DocumentStore,
//                                      new string[] {(RavenController.DocumentStore as DocumentStore).DefaultDatabase}); //TODO: robustify

        }

        //http://stackoverflow.com/a/8755565/214073
        public void CreateIndexesForDatabases(Assembly assemblyToScanForIndexingTasks, IDocumentStore documentStore, string[] databases)
        {
            var catalog = new CompositionContainer(new AssemblyCatalog(assemblyToScanForIndexingTasks));
            foreach (var database in databases)
            {
                IndexCreation.CreateIndexes(catalog, documentStore.DatabaseCommands.ForDatabase(database), documentStore.Conventions);

                HackSecurity(database);
                HackLevels(database);
                HackDivisions(database);
            }
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
        

        //TODO: come up with something better and remove this
        private void HackSecurity(string database)
        {
            var session = RavenController.DocumentStore.OpenSession(database);
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
        private void HackLevels(string database)
        {
            var session = RavenController.DocumentStore.OpenSession(database);
            var ok = session.Query<Level>().Any();
            if (ok) return;

            var levels = new List<Level>()
                             {
                                 new Level {Id = "levels-level1", Name = "All-Star Level 1", DefaultScoringDefinition = "scoring-level1"},
                                 new Level {Id = "levels-level2", Name = "All-Star Level 2", DefaultScoringDefinition = "scoring-level2"},
                                 new Level {Id = "levels-level3", Name = "All-Star Level 3", DefaultScoringDefinition = "scoring-level3"},
                                 new Level {Id = "levels-level4", Name = "All-Star Level 4", DefaultScoringDefinition = "scoring-level4"},
                                 new Level {Id = "levels-level5", Name = "All-Star Level 5", DefaultScoringDefinition = "scoring-level5"},
                                 new Level {Id = "levels-level6", Name = "All-Star Level 6", DefaultScoringDefinition = "scoring-level6"},
                                 new Level {Id = "levels-dance", Name = "Dance", DefaultScoringDefinition = "scoring-dance"},
                                 new Level {Id = "levels-school", Name = "School", DefaultScoringDefinition = "scoring-school"},
                                 new Level {Id = "levels-individual", Name = "Individual", DefaultScoringDefinition = "scoring-individual"}
                             };
            
            levels.ForEach(session.Store);
            session.SaveChanges();
        }

        //TODO: come up with something better and remove this
        private void HackDivisions(string database)
        {
            var session = RavenController.DocumentStore.OpenSession(database);
            var ok = session.Query<Division>().Any();
            if (ok) return;

            var levels = session.Query<Level>().ToList();

            var divisions = new List<Division>()
                             {
                                 new Division{ Name = "Small Youth", LevelId = "levels-level1"},
                                 new Division{ Name = "Large Youth", LevelId = "levels-level2"},
                                 new Division{ Name = "Small Junior", LevelId = "levels-level3"},
                                 new Division{ Name = "Large Junior", LevelId = "levels-level4"},
                                 new Division{ Name = "Small Senior", LevelId = "levels-level5"},
                                 new Division{ Name = "Large Senior", LevelId = "levels-level6"}
                             };

            divisions.ForEach(session.Store);
            session.SaveChanges();
        }
    }
}