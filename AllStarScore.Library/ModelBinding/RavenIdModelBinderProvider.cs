using System;
using System.Web.Mvc;

namespace AllStarScore.Library.ModelBinding
{
    public class RavenIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            return new RavenIdModelBinder();
        }
    }
}