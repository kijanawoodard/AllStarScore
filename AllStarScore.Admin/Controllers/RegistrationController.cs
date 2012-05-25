using System.Web.Mvc;
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
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Register(RegistrationRegisterViewModel model)
        {
            return PartialView(model);
        }
    }
}