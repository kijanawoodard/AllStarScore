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
                    .As<DivisionWithLevelsViewModel>()
                    .ToList();

            var teams = new List<TeamRegistration>()
                            {
                                new TeamRegistration()
                                    {CompetitionId = 1, DivisionId = "1", GymId = 1, Id = "d/1", TeamName = "Furious", IsShowTeam = true},
                                new TeamRegistration()
                                    {CompetitionId = 1, DivisionId = "1", GymId = 2, Id = "d/2", TeamName = "Five", IsShowTeam = false}
                            };

            model.Teams = teams;
            model.Divisions = divisions;

            return PartialView(model);
        }

        [HttpPost]
        public JsonDotNetResult Create(RegistrationCreateCommand command)
        {
            return Execute(() =>
                               {
                                   var registration = new TeamRegistration();
                                   registration.Update(command);

                                   //RavenSession.Store(registration);
                                   return new JsonDotNetResult(registration);
                               });
        }

        [HttpGet]
        public ActionResult Register(RegistrationRegisterViewModel model)
        {
            return PartialView(model);
        }
    }
}