using System.Web.Mvc;
using Moth.Core;

namespace AllStarScore.Library.Moth
{
    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!(filterContext.Controller is ResourcesController))
                base.OnAuthorization(filterContext);
        }
    }
}