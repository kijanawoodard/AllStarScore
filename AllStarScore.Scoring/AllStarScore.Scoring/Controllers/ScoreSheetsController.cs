using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class ScoreSheetsController : RavenController
    {
        public ActionResult Index(string id)
        {
            var import =
                RavenSession
                    .Load<CompetitionInfo>(id);

            var model = new ScoreSheetsIndexViewModel(import);
            return View(model);
        }
    }
}
