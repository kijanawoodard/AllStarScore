using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            var teams = new List<TeamRegistration>()
                            {
                                new TeamRegistration()
                                    {CompetitionId = 1, DivisionId = 1, GymId = 1, Id = "d/1", Name = "Furious", IsShowTeam = true},
                                new TeamRegistration()
                                    {CompetitionId = 1, DivisionId = 1, GymId = 2, Id = "d/2", Name = "Five", IsShowTeam = false}
                            };

            model.Teams = JsonConvert.SerializeObject(teams, Formatting.None, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return PartialView(model);
        }

        [HttpGet]
        public string TeamRegistrations()
        {
            var teams = new List<TeamRegistration>()
                            {
                                new TeamRegistration()
                                    {CompetitionId = 1, DivisionId = 1, GymId = 1, Id = "d/1", Name = "Furious"},
                                new TeamRegistration()
                                    {CompetitionId = 1, DivisionId = 2, GymId = 2, Id = "d/2", Name = "Five"}
                            };

            return new JavaScriptSerializer().Serialize(teams);
        }

        [HttpPost]
        public JsonResult Create(RegistrationCreateCommand command)
        {
            return Json(command, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Register(RegistrationRegisterViewModel model)
        {
            return PartialView(model);
        }
    }
}