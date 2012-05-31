using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Infrastructure.Utilities;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class CompetitionController : RavenController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListHarness()
        {
            return PartialView();
        }
        public ActionResult List()
        {
            var competitions = RavenSession
                                .Query<Competition>()
                                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                .ToList();

            var model = new CompetitionListViewModel(competitions);
            return PartialView(model);
        }

        public ActionResult Details(string id)
        {
            var competition = RavenSession.Load<Competition>(id);

            var stats =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationByGym>()
                    .Where(x => x.CompetitionId == id)
                    .As<TeamRegistrationByGym.Results>()
                    .ToList();

            var model = new CompetitionDetailsViewModel(competition, stats);
            return View(model);
        }

        public ActionResult CreateHarness()
        {
            return PartialView();
        }
        public ActionResult Create()
        {
            var model = new CompetitionCreateCommand();
            return PartialView(model);
        }

        [HttpPost]
        public JsonDotNetResult Create(CompetitionCreateCommand command)
        {
            return Execute(
                action: () =>
                {
                    var competition = new Competition();
                    competition.Update(command);
                    RavenSession.Store(competition);
                    return new JsonDotNetResult(competition);
                });
        }
    }
}
