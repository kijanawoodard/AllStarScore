using System;
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
            //TODO: can i make load starting with work with Lazily. See if it's already there in 1.2
            var competitions =
                RavenSession
                    .Query<Competition, CompetitionSearch>()
                    .Where(x => x.CompanyId == CurrentCompanyId)
                    .Lazily();

            var stats =
                RavenSession
                    .Query<Registration, TeamRegistrationStatsByCompetition>()
                    .Where(x => x.CompanyId == CurrentCompanyId)
                    .As<TeamRegistrationStatsByCompetition.Results>()
                    .ToList();

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
                    .Except(stats) //if we have stats, drop it
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
            return ExecuteInExplicitTransaction(
                action: () =>
                {
                    var competition = new Competition();
                    competition.Update(command);
                    RavenSession.Store(competition);
                    RavenSession.SaveChanges();

                    var scheduleCreateCommand = new ScheduleCreateCommand
                                                {
                                                    CompetitionId = competition.Id,
                                                    Days = competition.Days
                                                };

                    scheduleCreateCommand.CopyCommandPropertiesFrom(command);

                    var schedule = new Schedule();
                    schedule.Update(scheduleCreateCommand);
                    RavenSession.Store(schedule);
                    RavenSession.SaveChanges();

                    var url = Url.Action("Details", "Competition", new {id = competition.Id.ForMvc()});
                    return new JsonDotNetResult(url);
                });
        }

        public ActionResult Details(string id)
        {
            var stats =
                RavenSession
                    .Query<Registration, TeamRegistrationStatsByGym>()
                    .Customize(x => x.Include<TeamRegistrationStatsByGym.Results>(y => y.CompetitionId))
                    .Where(x => x.CompetitionId == id)
                    .As<TeamRegistrationStatsByGym.Results>()
                    .ToList();

            var competition = 
                RavenSession
                    .Load<Competition>(id);

            if (competition == null)
                return new HttpNotFoundResult();

            var model = new CompetitionDetailsViewModel(competition, stats);
            return View(model);
        }
    }
}
