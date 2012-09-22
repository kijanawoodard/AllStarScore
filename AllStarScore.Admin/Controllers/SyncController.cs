using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
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
            var competition =
                RavenSession
                    .Load<Competition>(id);

            var model = new SyncIndexViewModel(competition);
            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Export(ExportRequestModel request)
        {
            var company =
                RavenSession.Query<Company>()
                    .First();

            var registrations =
                RavenSession
                    .Query<Registration, TeamRegistrationByCompetition>()
                    .Where(x => x.CompetitionId == request.CompetitionId)
                    .Take(int.MaxValue)
                    .As<TeamRegistrationByCompetitionResults>()
                    .OrderBy(x => x.CreatedAt)
                    .Lazily();

            var competition =
                RavenSession
                    .Advanced.Lazily
                    .Load<Competition>(request.CompetitionId);

            var divisions =
                RavenSession
                    .Query<Division>()
                    .Take(int.MaxValue)
                    .Lazily();

            var levels =
                RavenSession
                    .Query<Level>()
                    .Take(int.MaxValue)
                    .Lazily();
                    
            var schedule = RavenSession
                            .Query<Schedule, ScheduleByCompetition>()
                            .FirstOrDefault(x => x.CompetitionId == request.CompetitionId);

            if (schedule == null)
                return new HttpNotFoundResult();

            var model = new CompetitionInfo
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

                            Company = company,
                            Competition = competition.Value,
                            Registrations = registrations.Value.ToList(),
                            Schedule = schedule,
                            CompetitionId = competition.Value.Id,
                            CompetitionName = competition.Value.Name,
                            CompetitionDescription = competition.Value.Description,
                            Days = competition.Value.Days.ToList(),
                            Divisions = divisions.Value.ToList(),
                            Levels = levels.Value.ToList()
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
