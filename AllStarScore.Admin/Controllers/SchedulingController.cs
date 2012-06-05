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
    public class SchedulingController : RavenController
    {
        public ActionResult Edit(string id)
        {
            var registrations =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationByCompetition>()
                    .As<TeamRegistrationByCompetition.Results>()
                    .ToList();

            var model = new SchedulingEditViewModel(registrations);
            return View(model);
        }

    }
}
