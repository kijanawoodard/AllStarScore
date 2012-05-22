using System.Web.Mvc;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Controllers
{
    public class RegistrationController : RavenController
    {
        public ActionResult Register(int id)
        {
            var competition = RavenSession
                                    .Load<Competition>(id);

            var model = new RegistrationRegisterViewModel(competition);
            return View(model);
        }

        [HttpGet]
        public ActionResult Teams(Foo foo)
        {
            var model = new RegistrationTeamsViewModel(foo.CompetitionId, foo.GymId);
            return PartialView(model);
        }
    }

    public class Foo
    {
        public int CompetitionId { get; set; }
        public int GymId { get; set; }
    }
}