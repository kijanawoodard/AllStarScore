using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class SyncController : RavenController
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            var model = new SyncIndexViewModel(id);
            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Export(ExportRequestModel request)
        {
            var registrations =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationByCompetition>()
                    .Take(int.MaxValue)
                    .As<TeamRegistrationByCompetition.Results>()
                    .OrderBy(x => x.RegistrationDate)
                    .Lazily();

            var competition =
                RavenSession
                    .Advanced.Lazily
                    .Load<Competition>(request.CompetitionId);

            var schedule = RavenSession
                            .Query<Schedule, ScheduleByCompetition>()
                            .FirstOrDefault(x => x.CompetitionId == request.CompetitionId);

            if (schedule == null)
                return new HttpNotFoundResult();

            var model = new ScoringImportData
                        {
                            Performances = schedule
                                .PerformanceEntries
                                .Select(entry =>
                                {
                                    var registration = registrations.Value.First(r => r.Id == entry.RegistrationId);
                                    return new Performance
                                           {
                                               CompetitionId = competition.Value.Id,
                                               RegistrationId = registration.Id,
                                               GymId = registration.GymId,
                                               DivisionId = registration.DivisionId,
                                               LevelId = registration.LevelId,
                                               PerformanceTime = entry.PerformanceTime,
                                               Panel = entry.Panel,
                                               WarmupTime = entry.WarmupTime,
                                               Duration = entry.Duration,
                                               GymName = registration.GymName,
                                               TeamName = registration.TeamName,
                                               DivisionName = registration.DivisionName,
                                               LevelName = registration.LevelName,
                                               IsSmallGym = registration.IsSmallGym,
                                               IsShowTeam = registration.IsShowTeam,
                                               GymLocation = registration.GymLocation,
                                               ParticipantCount = registration.ParticipantCount,
                                           };
                                })
                                .ToList(),

                            CompanyName = "Spirit Celebration", //TODO: Create Document
                            CompetitionId = competition.Value.Id,
                            CompetitionDescription = competition.Value.Description,
                            Days = competition.Value.Days.ToList()
                        };
            

            return new JsonDotNetResult(model);
        }

        public class ExportRequestModel
        {
            public string CompetitionId { get; set; }
            public string Hash { get; set; }
        }
    }
}
