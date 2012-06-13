using System;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Extensions;
using Raven.Client;

namespace AllStarScore.Scoring.Controllers
{
    //https://github.com/ayende/RaccoonBlog/blob/master/HibernatingRhinos.Loci.Common/Controllers/RavenController.cs
    public abstract class RavenController : Controller
    {
        public static IDocumentStore DocumentStore
        {
            get { return _documentStore; }
            set
            {
                if (_documentStore == null)
                {
                    _documentStore = value;
                }
            }
        }
        private static IDocumentStore _documentStore;

        public IDocumentSession RavenSession { get; protected set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = _documentStore.OpenSession(); //TODO: make this dynamic based on ip
        }

        // TODO: Consider re-applying https://github.com/ayende/RaccoonBlog/commit/ff954e563e6996d44eb59a28f0abb2d3d9305ffe
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            CompleteSessionHandler(filterContext);
        }

        protected ActionResult Execute(Action action, Func<ActionResult> onsuccess, Func<ActionResult> onfailure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    action();
                    return onsuccess();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "An unexpected error occurred and has been logged. Please try again later" + e.Message);
                //TODO: Make the above assertion true
                throw;
            }

            return onfailure();
        }

        protected JsonDotNetResult Execute(Func<JsonDotNetResult> action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return action();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            var errors = ModelState
                            .ToDictionary(kvp => kvp.Key,
                                          kvp => kvp.Value.Errors
                                                    .Select(e => e.ErrorMessage)
                                                    .ToArray())
                            .Where(m => m.Value.Any());

            HttpContext.Response.StatusCode = 400;
            return new JsonDotNetResult(new { errors });
        }

        protected void CompleteSessionHandler(ActionExecutedContext filterContext)
        {
            using (RavenSession)
            {
                if (filterContext.Exception != null)
                    return;

                if (RavenSession != null)
                    RavenSession.SaveChanges();
            }

//            TaskExecutor.StartExecuting();
        }

        protected HttpStatusCodeResult HttpNotModified()
        {
            return new HttpStatusCodeResult(304);
        }
    }

    //http://stackoverflow.com/a/7382312/214073
    public class JsonDotNetResult : ActionResult
    {
        private readonly object _obj;
        public JsonDotNetResult(object obj)
        {
            _obj = obj;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.AddHeader("content-type", "application/json");
            context.HttpContext.Response.Write(_obj.ToJson()); //I'm using this extension method so I get consistent formatting on the property names
        }
    }
}