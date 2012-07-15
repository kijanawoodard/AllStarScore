using System;

namespace AllStarScore.Library
{
    public interface ITenantProvider
    {
        string GetCompanyId(Uri uri);
        void SetCompanyId(Uri uri, string companyId);
        string GetKey(Uri uri);

        void EnsureTenantMapExists();
    }
}