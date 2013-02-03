using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;

namespace AllStarScore.Admin.Controllers
{
	public class ScoreSheetsController : RavenController
	{
		[HttpGet]
		public ActionResult Index(string id)
		{
			var competition =
			   RavenSession
				   .Load<Competition>(id);

			var model = new ScoreSheetsIndexViewModel(competition);
			return View(model);
		}
	}
}