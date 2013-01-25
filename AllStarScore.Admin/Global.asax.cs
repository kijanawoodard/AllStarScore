using System.Diagnostics;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AllStarScore.Library.ModelBinding;
using AllStarScore.Library.Moth;
using Moth.Core;
using RouteMagic;

namespace AllStarScore.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
//        public MvcApplication()
//        {
//            //https://github.com/ayende/RaccoonBlog/blob/master/RaccoonBlog.Web/Global.asax.cs
//            BeginRequest += (sender, args) =>
//            {
////                HttpContext.Current.Items["CurrentRequestRavenSession"] = RavenController.DocumentStore.OpenSession();
//            };
//
//            EndRequest += (sender, args) =>
//            {
//                using (var session = ObjectFactory.GetInstance<IDocumentSession>())
//                {
//                    if (session == null)
//                        return;
//
//                    if (Server.GetLastError() != null)
//                        return;
//
//                    session.SaveChanges();
//                }
//                //  TaskExecutor.StartExecuting();
//            };
//        }

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

            images = routes.MapRoute("content_jqueryui_images", "content/themes/base/images/{file}");
            routes.Redirect(r => r.MapRoute("moth_jqueryui_images", "resources/css/images/{file}"))
                .To(images);


//            routes.MapHttpRoute(
//                name: "DefaultApi",
//                routeTemplate: "api/{controller}/{id}",
//                defaults: new { id = RouteParameter.Optional }
//            );

            routes.MapRoute(
                name: null,
                url: "download/{token}/{competitionid}",
                defaults: new { controller = "Sync", action = "Download" }
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
                defaults: new { controller = "Competition", action = "Index", id = UrlParameter.Optional }
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
            ModelBinderProviders.BinderProviders.Insert(0, new RavenIdModelBinderProvider());
        }

//        [Conditional("DEBUG")]
        private void InitializeMothForDebugging()
        {
            MothAction.Initialize(new CustomMothProvider());
        }
    }
}