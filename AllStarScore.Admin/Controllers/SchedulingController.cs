using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Infrastructure.Utilities;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class SchedulingController : RavenController
    {
        public ActionResult Edit(string id)
        {
            var competition =
                RavenSession.Load<Competition>(id);

            var registrations =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationByCompetition>()
                    .As<TeamRegistrationByCompetition.Results>()
                    .ToList();

            var schedule = new Schedule(competition.FirstDay.GetDateRange(competition.LastDay));

            var model = new SchedulingEditViewModel(schedule, registrations);
            return View(model);
        }

    }
}
