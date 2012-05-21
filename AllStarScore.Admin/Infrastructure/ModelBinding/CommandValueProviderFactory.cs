using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllStarScore.Admin.Infrastructure.ModelBinding
{
    public class CommandValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            var nvc = new NameValueCollection
                          {
                              {"CommandByUser", controllerContext.HttpContext.User.Identity.Name},
                              {"CommandWhen", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}
                          };

            return new NameValueCollectionValueProvider(nvc, CultureInfo.InvariantCulture);
        }
    }
}