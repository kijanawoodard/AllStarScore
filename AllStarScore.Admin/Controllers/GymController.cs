using System;
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
            GymCreateSuccessfulViewModel model = null;
            ModelState.AddModelError("", "bah");
            return Execute(
                action: () => {
                                 var gym = new Gym();
                                 gym.Update(command);
                                 RavenSession.Store(gym);

                                  model = new GymCreateSuccessfulViewModel(gym.Id, gym.Name);
                },
                onsuccess: () => PartialView("CreateSuccessful", model),
                onfailure: () => PartialView(command));
        }

        [HttpGet]
        public ActionResult Search(string query)
        {
            //http://daniellang.net/searching-on-string-properties-in-ravendb/ - Did it the *wrong* way ;-)
            var gyms = RavenSession
                        .Advanced.LuceneQuery<Gym, GymsByName>()
                        .Where("Name: *" + query + "* OR Location: *" + query + "*" )
                        .ToList();

            return Json(gyms, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Check(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Content("");

            var gyms = RavenSession
                            .Query<Gym, GymsByName>()
                            .Where(x => x.Name == name) //Index returns matches on each term which isn't what we want here. Exact match below
                            .ToList();

            var gym = gyms.SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            var view = gym == null ? "GymNameAvailable" : "GymNameTaken";
            var model = gym == null ? 0 : gym.Id;
            return PartialView(view, model);
        }

        [HttpGet]
        public ActionResult Details(int gymid)
        {
            var gym = RavenSession
                            .Load<Gym>(gymid);

            var model = new GymDetailsViewModel(gym);
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Edit(int gymid)
        {
            var gym = RavenSession
                            .Load<Gym>(gymid);

            var model = new GymEditCommand(gym);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(GymEditCommand command)
        {
            return Execute(
                action: () =>
                {
                    var gym = RavenSession
                            .Load<Gym>(command.Id);            

                    gym.Update(command);
                },
                onsuccess: () => { ViewBag.Success = true;
                                     return PartialView(command);
                }, //PartialView("EditSuccessful", new GymEditSuccessfulViewModel(command.Id)),
                onfailure: () => PartialView(command));
        }
    }
}
