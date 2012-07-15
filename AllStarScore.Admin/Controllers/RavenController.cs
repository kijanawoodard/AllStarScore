using System;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Extensions;
using Raven.Client;

namespace AllStarScore.Admin.Controllers
{
    public class RavenController : Controller
    {
        //https://github.com/ayende/RaccoonBlog/blob/master/HibernatingRhinos.Loci.Common/Controllers/RavenController.cs
    
        public IDocumentSession RavenSession { get; set; }

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

        protected ActionResult Execute(Action action, Func<ActionResult> onsuccess)
        {
            return Execute(action, onsuccess, onsuccess);
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

        protected HttpStatusCodeResult HttpNotModified()
        {
            return new HttpStatusCodeResult(304);
        }
    }
    
    public class JsonDotNetResult : ActionResult
    {
        //http://stackoverflow.com/a/7382312/214073

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

//posterity

//        protected ActionResult Execute(Action action, Func<ActionResult> onsuccess, Func<ActionResult> onfailure)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    action();
//                    return onsuccess();
//                }
//            }
//            catch (Exception e)
//            {
//                ModelState.AddModelError("", "An unexpected error occurred and has been logged. Please try again later" + e.Message);
//                //TODO: Make the above assertion true
//                throw;
//            }
//
//            return onfailure();
//        }
