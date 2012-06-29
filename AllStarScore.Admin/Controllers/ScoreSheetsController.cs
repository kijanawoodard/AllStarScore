using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class ScoreSheetsController : RavenController
    {
        public ActionResult Details(string id)
        {
            var registrations =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationByCompetition>()
                    .As<TeamRegistrationByCompetition.Results>()
                    .OrderBy(x => x.CreatedAt)
                    .Lazily();

            var competition =
               RavenSession
                   .Advanced.Lazily
                   .Load<Competition>(id);

            var schedule =
                RavenSession
                    .Query<Schedule, ScheduleByCompetition>()
                    .FirstOrDefault(x => x.CompetitionId == id);

            var model = new ScoreSheetsDetailsViewModel(schedule, registrations.Value, competition.Value);
            return View(model);
        }

    }
}
