using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;

namespace AllStarScore.Library.ModelBinding
{
    public class CommandValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            var nvc = new NameValueCollection
                      {
                          {"CommandByUser", controllerContext.HttpContext.User.Identity.Name},
                          {"CommandWhen", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}
                      };

            return new NameValueCollectionValueProvider(nvc, CultureInfo.InvariantCulture);
        }
    }

    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult =
                bindingContext
                    .ValueProvider
                    .GetValue(bindingContext.ModelName);

            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
                                                CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}