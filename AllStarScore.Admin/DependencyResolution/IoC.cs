using AllStarScore.Admin.Controllers;
using Raven.Client;
using StructureMap;

namespace AllStarScore.Admin.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
//                    scan.AddAllTypesOf<RavenController>();
                });

                x.AddRegistry(new RavenDbRegistry("RavenDB"));
                
                x.SetAllProperties(c => c.OfType<IDocumentSession>());
            });
            return ObjectFactory.Container;
        }
    }
}