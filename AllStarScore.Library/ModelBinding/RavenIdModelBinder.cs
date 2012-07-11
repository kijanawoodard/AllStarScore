using System.Web.Mvc;
using AllStarScore.Extensions;

namespace AllStarScore.Library.ModelBinding
{
    public class RavenIdModelBinder : DefaultModelBinder, IModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = base.BindModel(controllerContext, bindingContext);

            //convert any id fields back to RavenFormat
            if (bindingContext.ModelName.ToLower().EndsWith("id") && result is string)
            {
                result = (result as string).FromMvc();
            }

            return result;
        }
    }
}