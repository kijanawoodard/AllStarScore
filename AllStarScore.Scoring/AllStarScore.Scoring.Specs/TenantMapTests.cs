using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllStarScore.Library;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace AllStarScore.Scoring.Specs
{
    [TestFixture]
    public class TenantMapTests
    {
        protected EmbeddableDocumentStore DocumentStore { get; private set; }
        
        [TestFixtureSetUp]
        public void Setup()
        {
            DocumentStore = new EmbeddableDocumentStore { RunInMemory = true };
            DocumentStore.RegisterListener(new NoStaleQueriesAllowed());
            DocumentStore.Initialize();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            DocumentStore.Dispose();
        }

        [Test]
        public void UnregisteredDomainReturnsNull()
        {
            var url = string.Format("http://www.{0}.com", Guid.NewGuid());
            var uri = new Uri(url);

            using (var session = DocumentStore.OpenSession())
            {
                var provider = new TenantProvider(session);
                var id = provider.GetCompanyId(uri);
                Assert.IsNull(id);
            }
        }

        [Test]
        public void WhenIAddAnIdToAnUnregisteredDomainICanRetrieveThatId()
        {
            const string id = "company/1";
            var url = string.Format("http://www.{0}.com", Guid.NewGuid());
            var uri = new Uri(url);

            using (var session = DocumentStore.OpenSession())
            {
                var provider = new TenantProvider(session);
                provider.SetCompanyId(uri, id);
            }

            using (var session = DocumentStore.OpenSession())
            {
                var provider = new TenantProvider(session);
                Assert.AreEqual(id, provider.GetCompanyId(uri));
            }
        }

        [Test]
        public void WhenIUpdateARegisteredDomainICanRetrieveThatId()
        {
            var id = "company/1";
            var url = string.Format("http://www.{0}.com", Guid.NewGuid());
            var uri = new Uri(url);
                
            using (var session = DocumentStore.OpenSession())
            {
                var provider = new TenantProvider(session);
                provider.SetCompanyId(uri, id);
            }

            using (var session = DocumentStore.OpenSession())
            {
                var provider = new TenantProvider(session);
                Assert.AreEqual(id, provider.GetCompanyId(uri));
            }

            id = "company/9";

            using (var session = DocumentStore.OpenSession())
            {
                var provider = new TenantProvider(session);
                provider.SetCompanyId(uri, id);
            }

            using (var session = DocumentStore.OpenSession())
            {
                var provider = new TenantProvider(session);
                Assert.AreEqual(id, provider.GetCompanyId(uri));
            }
        }
    }
}
