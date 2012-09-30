using System.Web;
using System.Web.Mvc;
using AllStarScore.Extensions;
using StructureMap;

namespace AllStarScore.Library.ModelBinding
{
    public class RavenIdModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var tenants = ObjectFactory.GetInstance<ITenantProvider>(); //doing this to avoid keeping a session around for the lifetime of the app or creating a new session from doc store
            var company = tenants.GetCompanyId(HttpContext.Current.Request.Url);

            var result = base.BindModel(controllerContext, bindingContext);

            //convert any id fields back to RavenFormat
            if (bindingContext.ModelName.ToLower().EndsWith("id") && result is string)
            {
                var working = result.FromMvc();
                if (!working.StartsWith(company))
                    working = company + "/" + working;

                result = working;
            }

            return result;
        }
    }


	public class SimpleRavenIdModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var result = base.BindModel(controllerContext, bindingContext);

			//convert any id fields back to RavenFormat
			if (bindingContext.ModelName.ToLower().EndsWith("id") && result is string)
			{
				result = result.FromMvc();
			}

			return result;
		}
	}
}