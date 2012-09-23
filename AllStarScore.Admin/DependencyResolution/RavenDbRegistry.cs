using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Models;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.UniqueConstraints;
using StructureMap.Configuration.DSL;

namespace AllStarScore.Admin.DependencyResolution
{
    public class RavenDbRegistry : Registry
    {
        private static Expression<Func<object, string>> _defaultKeyGenerator;

        public RavenDbRegistry(string connectionStringName)
        {
            // https://github.com/PureKrome/RavenOverflow/blob/master/Code/RavenOverflow.Web/DependencyResolution/RavenDbRegistry.cs

            For<IDocumentStore>()
                .Singleton()
                .Use(x =>
                {
                	var connectionString = ConfigurationManager.AppSettings[connectionStringName];
                    var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionString(connectionString);
                    parser.Parse();

                    var documentStore = new DocumentStore
                                        {
                                            ApiKey = parser.ConnectionStringOptions.ApiKey,
                                            Url = parser.ConnectionStringOptions.Url,
                                            
                                        };

                    var generator = new MultiTypeHiLoKeyGenerator(documentStore, 32);
                    documentStore.Conventions.DocumentKeyGenerator = entity =>
                    {
                        var special = entity as IGenerateMyId;
                        return special == null ? generator.GenerateDocumentKey(documentStore.Conventions, entity) : special.GenerateId();
                    };

                    documentStore.RegisterListener(new UniqueConstraintsStoreListener());

                    documentStore.Initialize();
                    InitializeRavenProfiler(documentStore);
                    IndexCreation.CreateIndexes(typeof(CompetitionSearch).Assembly, documentStore);

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