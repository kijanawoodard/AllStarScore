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
            var registrations =
                RavenSession
                    .Query<TeamRegistration, TeamRegistrationByCompetition>()
                    .As<TeamRegistrationByCompetition.Results>()
                    .OrderBy(x => x.RegistrationDate)
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
                    .Query<Schedule, ScheduleByCompetition>()
                    .FirstOrDefault(x => x.CompetitionId == id);

            if (schedule == null)
            {
                schedule = new Schedule(competition.Value);   
                RavenSession.Store(schedule);
            }

            var model = new SchedulingEditViewModel();
            model.Schedule = schedule;
            
            model.Registrations = registrations.Value.ToList();
            model.Divisions = divisions.Value.ToList();
            model.CompetitionDays = competition.Value.Days;

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

                    return new JsonDotNetResult(true);
                });
        }
    }
}
