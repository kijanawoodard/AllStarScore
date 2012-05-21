using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;
using AllStarScore.Admin.ViewModels;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class GymController : RavenController
    {
        [HttpGet]
        public ActionResult Create()
        {
            var model = new GymCreateCommand();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(GymCreateCommand command)
        {
            return Execute(
                action: () => {
                                 var gym = new Gym();
                                 gym.Update(command);
                                 RavenSession.Store(gym);
                              },
                onsuccess: () => PartialView("CreateSuccessful", command),
                onfailure: () => PartialView(command));
        }

        [HttpGet]
        public ActionResult Search(string query)
        {
            var gyms = RavenSession
                            .Query<Gym, GymsByName>()
                            .Where(gym => gym.Name == query);

            return Json(gyms, JsonRequestBehavior.AllowGet);
        }
    }
}
