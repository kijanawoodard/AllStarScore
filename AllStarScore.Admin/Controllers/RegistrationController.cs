using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;

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
                                    {CompetitionId = 1, Division = "div 1", GymId = 1, Id = 1, Name = "Furious"},
                                new TeamRegistration()
                                    {CompetitionId = 1, Division = "div 2", GymId = 2, Id = 2, Name = "Five"}
                            };

            model.Teams = teams;
            return PartialView(model);
        }

        [HttpGet]
        public string TeamRegistrations()
        {
            var teams = new List<TeamRegistration>()
                            {
                                new TeamRegistration()
                                    {CompetitionId = 1, Division = "div 1", GymId = 1, Id = 1, Name = "Furious"},
                                new TeamRegistration()
                                    {CompetitionId = 1, Division = "div 2", GymId = 2, Id = 2, Name = "Five"}
                            };

            return new JavaScriptSerializer().Serialize(teams);
        }


        [HttpGet]
        public ActionResult Register(RegistrationRegisterViewModel model)
        {
            return PartialView(model);
        }
    }
}