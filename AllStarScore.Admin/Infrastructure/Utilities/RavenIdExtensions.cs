using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllStarScore.Admin.Infrastructure.Utilities
{
    public static class RavenIdExtensions
    {
        public static int ToRavenInteger(this string ravenId)
        {
            return ravenId.Substring(ravenId.IndexOf("/", StringComparison.InvariantCulture) + 1).ToInt();
        }
    }
}