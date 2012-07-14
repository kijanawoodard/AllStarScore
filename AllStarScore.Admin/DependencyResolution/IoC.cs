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
                    scan.WithDefaultConventions();
                });

                x.AddRegistry(new RavenDbRegistry("RavenDB"));
                
                x.SetAllProperties(c => c.OfType<IDocumentSession>());
            });
            return ObjectFactory.Container;
        }
    }
}