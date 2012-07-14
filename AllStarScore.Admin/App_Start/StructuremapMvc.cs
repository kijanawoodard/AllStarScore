using System.Web.Mvc;
using AllStarScore.Admin.DependencyResolution;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AllStarScore.Admin.App_Start.StructuremapMvc), "Start")]
namespace AllStarScore.Admin.App_Start
{
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var container = IoC.Initialize();
            var resolver = new SmDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
        }
    }
}