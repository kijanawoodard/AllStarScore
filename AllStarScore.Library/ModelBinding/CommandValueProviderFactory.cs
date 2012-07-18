using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace AllStarScore.Library.ModelBinding
{
    public class CommandValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            var tenants = ObjectFactory.GetInstance<ITenantProvider>(); //doing this to avoid keeping a session around for the lifetime of the app or creating a new session from doc store
            var nvc = new NameValueCollection
                      {
                          {"commandCompanyId", tenants.GetCompanyId(HttpContext.Current.Request.Url)},
                          {"CommandByUser", controllerContext.HttpContext.User.Identity.Name},
                          {"CommandWhen", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}
                      };

            return new NameValueCollectionValueProvider(nvc, CultureInfo.InvariantCulture);
        }
    }
}