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

        public ActionResult List()
        {
            var competitions = RavenSession
                                .Query<Competition>()
                                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                .Lazily();

            var stats =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationStatsByCompetition>()
                    .As<TeamRegistrationStatsByCompetition.Results>()
                    .ToList();

            var have = stats.Select(s => s.CompetitionId).ToList();
            var converted =
                competitions.Value
                    .Select(competition => new TeamRegistrationStatsByCompetition.Results
                                               {
                                                   CompetitionId = competition.Id,
                                                   CompetitionName = competition.Name,
                                                   CompetitionFirstDay = competition.FirstDay,
                                                   GymCount = 0,
                                                   TeamCount = 0,
                                                   ParticipantCount = 0
                                               })
                    .Except(stats) //if we have stats, frop it
                    .ToList();

            stats.AddRange(converted);


            var model = new CompetitionListViewModel(stats);
            return PartialView(model);
        }

        public ActionResult Details(string id)
        {
            var competition = RavenSession.Advanced.Lazily.Load<Competition>(id);

            var stats =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationStatsByGym>()
                    .Where(x => x.CompetitionId == id)
                    .As<TeamRegistrationStatsByGym.Results>()
                    .ToList();

            var model = new CompetitionDetailsViewModel(competition.Value, stats);
            return View(model);
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
