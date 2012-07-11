using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Extensions;
using AllStarScore.Models;
using AllStarScore.Models.Commands;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class CompetitionController : RavenController
    {
        public ActionResult Index()
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


            var model = new CompetitionIndexViewModel(stats);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
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

                    var url = Url.Action("Details", "Competition", new {id = competition.Id.ForMvc()});
                    return new JsonDotNetResult(url);
                });
        }

        public ActionResult Details(string id)
        {
            var stats =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationStatsByGym>()
                    .Customize(x => x.Include<TeamRegistrationStatsByGym.Results>(y => y.CompetitionId))
                    .Where(x => x.CompetitionId == id)
                    .As<TeamRegistrationStatsByGym.Results>()
                    .ToList();

            var competition = 
                RavenSession
                    .Load<Competition>(id);

            var model = new CompetitionDetailsViewModel(competition, stats);
            return View(model);
        }
    }
}
