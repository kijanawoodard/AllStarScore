using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AllStarScore.Extensions;

namespace AllStarScore.Scoring.Infrastructure.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static IHtmlString EncodeAsJson<T>(this HtmlHelper helper, T model)
		{
			//ToJson lower cases the properties so they are more javascript-y
			//encode takes care of quotes, line breaks, etc
			//raw so that it is not treated as html
			return helper.Raw(Json.Encode(model.ToJson()));
		}
	}
}