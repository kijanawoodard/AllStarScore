using System.Web.Mvc;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Controllers
{
    public class RegistrationController : RavenController
    {
        public ActionResult Register(int id)
        {
            var competitionName = RavenSession
                                    .Load<Competition>(id)
                                    .Name;

            var model = new RegistrationRegisterViewModel(competitionName);
            return View(model);
        }
    }
}