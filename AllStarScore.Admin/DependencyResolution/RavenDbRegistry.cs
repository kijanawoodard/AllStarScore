using System.Diagnostics;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Models;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using StructureMap.Configuration.DSL;

namespace AllStarScore.Admin.DependencyResolution
{
    public class RavenDbRegistry : Registry
    {
        public RavenDbRegistry(string connectionStringName)
        {
            // https://github.com/PureKrome/RavenOverflow/blob/master/Code/RavenOverflow.Web/DependencyResolution/RavenDbRegistry.cs

            For<IDocumentStore>()
                .Singleton()
                .Use(x =>
                {
                    var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName(connectionStringName);
                    parser.Parse();

                    var documentStore = new DocumentStore
                                        {
                                            ApiKey = parser.ConnectionStringOptions.ApiKey,
                                            Url = parser.ConnectionStringOptions.Url,
                                            
                                        };
                    
                    var defaultGenerator = documentStore.Conventions.DocumentKeyGenerator;
                    documentStore.Conventions.DocumentKeyGenerator = entity =>
                    {
                        var special = entity as IGenerateMyId;
                        return special == null ? defaultGenerator(entity) : special.GenerateId();
                    };

                    documentStore.Initialize();
                    InitializeRavenProfiler(documentStore);
                    IndexCreation.CreateIndexes(typeof(GymsByName).Assembly, documentStore);

                    return documentStore;
                }
                )
                .Named("RavenDB Document Store.");

            For<IDocumentSession>()
                .HttpContextScoped()
                .Use(x =>
                {
                    var documentStore = x.GetInstance<IDocumentStore>();
                    return documentStore.OpenSession();
                })
                .Named("RavenDb Session -> per Http Request.");
        }

        [Conditional("DEBUG")]
        private void InitializeRavenProfiler(IDocumentStore documentStore)
        {
            Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(documentStore);
        }
    }
}