using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;
using AllStarScore.Models.Commands;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class SchedulingController : RavenController
    {
        public ActionResult Edit(string id)
        {
            var registrations =
                RavenSession
                    .Query<Registration, TeamRegistrationByCompetition>()
                    .Take(int.MaxValue)
                    .Where(x => x.CompetitionId == id)
                    .As<TeamRegistrationByCompetitionResults>()
                    .OrderBy(x => x.CreatedAt)
                    .Lazily();

            var divisions =
               RavenSession
                   .Query<Division, DivisionsWithLevels>()
                   .Take(int.MaxValue) //there shouldn't be very many of these in practice
                   .As<DivisionViewModel>()
                   .Lazily();

            var competition =
                RavenSession
                    .Advanced.Lazily
                    .Load<Competition>(id);

            var schedule =
                RavenSession
                    .Advanced.Lazily
                    .Load<Schedule>(Schedule.FormatId(id));
            
            var model = new SchedulingEditViewModel(schedule.Value, competition.Value, registrations.Value, divisions.Value);
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
