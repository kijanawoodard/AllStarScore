using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace AllStarScore.Admin.DependencyResolution
{
    //http://code.google.com/p/mvccontrib/source/browse/trunk/src/MvcContrib.StructureMap/StructureMapControllerFactory.cs?spec=svn510&r=510
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext context, string controllerName)
        {
            Type controllerType = base.GetControllerType(context, controllerName);
            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}