using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Library.RavenDB;
using AllStarScore.Models;
using AllStarScore.Models.Commands;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class SchedulingController : RavenController
    {
        public ActionResult Edit(string id)
        {
            var competition =
                RavenSession
                    .Advanced.Lazily
                    .Load<Competition>(id);

            var schedule =
                RavenSession
                    .Advanced.Lazily
                    .Load<Schedule>(Schedule.FormatId(id));

            var levels = 
                RavenSession
                    .LoadStartingWith<Level>(Level.FormatId(CurrentCompanyId));

            var divisions =
                RavenSession
                    .LoadStartingWith<Division>(Division.FormatId(CurrentCompanyId));

            var gyms =
                RavenSession
                    .LoadStartingWith<Gym>(Gym.FormatId(CurrentCompanyId));
            
            var registrations =
                RavenSession
                    .LoadStartingWith<Registration>(Registration.FormatId(id))
                    .ToList();

            var performances =
                registrations
                    .SelectMany(x => x.GetPerformances(competition.Value))
                    .ToList();

            var model = new SchedulingEditViewModel(schedule.Value, competition.Value, levels, divisions, gyms, registrations, performances);
            return View(model);
        }

        [HttpPost]
        public JsonDotNetResult Edit(SchedulingEditCommand command)
        {
            return Execute(
                action: () =>
                {
                    var schedule = RavenSession.Load<Schedule>(command.Id);
                    schedule.Update(command);

                    RavenSession.SaveChanges();

                    return new JsonDotNetResult(true);
                });
        }
    }
}
