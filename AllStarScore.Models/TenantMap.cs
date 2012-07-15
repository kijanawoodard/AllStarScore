using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllStarScore.Models
{
    public class TenantMap
    {
        public string Id { get; set; }
        public IDictionary<string, string> Tenants;

        public TenantMap()
        {
            Tenants = new Dictionary<string, string>();
        }
    }
}
