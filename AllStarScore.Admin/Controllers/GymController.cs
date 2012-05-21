using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Controllers
{
    public class GymController : RavenController
    {
        [HttpGet]
        public ActionResult Create()
        {
            var model = new GymCreateInputModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(GymCreateInputModel input)
        {
            var gym = new Gym();
            gym.Update(input);
            //RavenSession.Store(gym);

            return PartialView("CreateSuccessful", input);
            return PartialView(input);
        }

        [HttpGet]
        public ActionResult Search(string query)
        {
            var gyms = new List<Gym>()
                           {
                               new Gym() {Name = "High Spirit", Location = "Forney, TX", Id=19},
                               new Gym() {Name = "Tiger Cheer", Location = "Richardson, TX", Id=55}
                           };

            var result = gyms.Select(gym => string.Format("{0} from {1}", gym.Name, gym.Location));
            return Json(gyms, JsonRequestBehavior.AllowGet);
        }
    }
}
