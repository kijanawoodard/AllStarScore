using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class RegistrationController : RavenController
    {
        public ActionResult Index(RegistrationIndexRequestModel request)
        {
            var competition = RavenSession
                                    .Load<Competition>(request.CompetitionId);

            var model = new RegistrationIndexViewModel(competition, request.GymId);
            return View(model);
        }

        [HttpGet]
        public ActionResult Teams(RegistrationTeamsViewModel model)
        {
            var divisions =
                RavenSession
                    .Query<Division, DivisionsWithLevels>()
                    .Take(int.MaxValue) //there shouldn't be very many of these in practice
                    .As<DivisionViewModel>()
                    .Lazily();

            var teams =
                RavenSession
                    .Query<TeamRegistration>()
                    .Where(t => t.CompetitionId == model.CompetitionId && t.GymId == model.GymId)
                    .Select(
                    t =>
                    new TeamRegistrationViewModel()
                    {
                        Id = t.Id,
                        DivisionId = t.DivisionId,
                        TeamName = t.TeamName,
                        ParticipantCount = t.ParticipantCount,
                        IsShowTeam = t.IsShowTeam
                    })
                    .Take(int.MaxValue) //there shouldn't be very many of these in practice
                    .ToList();

            model.Teams = teams;
            model.Divisions = divisions.Value.ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonDotNetResult Create(RegistrationCreateCommand command)
        {
            return Execute(
                action: () =>
                            {
                                var registration = new TeamRegistration();
                                registration.Update(command);

                                RavenSession.Store(registration);
                                return new JsonDotNetResult(registration);
                            });
        }

        [HttpPost]
        public JsonDotNetResult Edit(RegistrationEditCommand command)
        {
            return Execute(
                action: () =>
                {
                    var registration = RavenSession.Load<TeamRegistration>(command.Id);
                    registration.Update(command); //fyi; the registration has Id instead of RegistrationId, but we're not using this class on the client

                    return new JsonDotNetResult(true); 
                });
        }
    }
}