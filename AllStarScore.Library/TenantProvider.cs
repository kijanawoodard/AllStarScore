using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllStarScore.Models;
using Raven.Client;

namespace AllStarScore.Library
{
    public class TenantProvider : ITenantProvider
    {
        private const string TenantMapId = "tenants";
        private readonly IDocumentSession _session;

        public TenantProvider(IDocumentSession session)
        {
            _session = session;
        }

        public string GetCompanyId(Uri uri)
        {
//            using (_session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(30)))
//            {
                var map =
                   _session
                       .Load<TenantMap>(TenantMapId);

                var key = GetKey(uri);
                var result = map.Tenants.ContainsKey(key) ? map.Tenants[key] : null;
                return result;
//            }
        }

        public void SetCompanyId(Uri uri, string companyId)
        {
            using (_session.Advanced.DocumentStore.DisableAggressiveCaching())
            {
                var map =
                   _session
                       .Load<TenantMap>(TenantMapId);

                var key = GetKey(uri);
                map.Tenants[key] = companyId;
                _session.SaveChanges(); 
            }
        }

        public string GetKey(Uri uri)
        {
            return uri.Host;
        }

        public void EnsureTenantMapExists()
        {
            var map =
                   _session
                       .Load<TenantMap>(TenantMapId);

            if (map != null)
                return;

            map = new TenantMap()
                  {
                      Id = TenantMapId
                  };

           _session.Store(map);
           _session.SaveChanges();    
        }
    }
}
