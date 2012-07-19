using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;
using AllStarScore.Models.Commands;
using AllStarScore.Library.RavenDB;
using Raven.Client.Linq;

namespace AllStarScore.Admin.Controllers
{
    public class GymController : RavenController
    {
        [HttpGet]
        public ActionResult GymList()
        {
            var gyms =
                RavenSession
                    .LoadStartingWith<Gym>(Gym.FormatId(CurrentCompanyId))
                    .ToList();

            var model = new GymListViewModel(gyms);
            return PartialView(model);
        }

        //        [HttpGet]
        //        public ActionResult Create()
        //        {
        //            var model = new GymCreateCommand();
        //            return PartialView(model);
        //        }


        [HttpPost]
        public JsonDotNetResult Create(GymCreateCommand command)
        {
            return Execute(
                action: () =>
                            {
                                var gym = new Gym();
                                gym.Update(command);

                                RavenSession.Store(gym);
                                RavenSession.SaveChanges();

                                return new JsonDotNetResult(gym);
                            });
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var gym =
                RavenSession
                    .Load<Gym>(id);

            var model = new GymEditCommand(gym);
            return PartialView(model);
        }

        [HttpPost]
        public JsonDotNetResult Edit(GymEditCommand command)
        {
            return Execute(
                action: () =>
                {
                    var gym =
                        RavenSession
                            .Load<Gym>(command.GymId);

                    gym.Update(command);

                    RavenSession.SaveChanges();

                    return new JsonDotNetResult(command);
                });
        }
    }
}

//Left this here as an example to myself of how to do full text search with RavenDB


//        [HttpGet]
//        public ActionResult Search(string query)
//        {
//            //http://daniellang.net/searching-on-string-properties-in-ravendb/ - Did it the *wrong* way ;-)
//            var gyms = RavenSession
//                        .Advanced.LuceneQuery<Gym, GymsByName>()
//                        .Where("Name: *" + query + "* OR Location: *" + query + "*" )
//                        .ToList();
//
//            return Json(gyms, JsonRequestBehavior.AllowGet);
//        }
//
//        [HttpGet]
//        public ActionResult Check(string name)
//        {
//            if (string.IsNullOrWhiteSpace(name)) return Content("");
//
//            var gyms = RavenSession
//                            .Query<Gym, GymsByName>()
//                            .Where(x => x.Name == name) //Index returns matches on each term which isn't what we want here. Exact match below
//                            .ToList();
//
//            var gym = gyms.SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
//            var view = gym == null ? "GymNameAvailable" : "GymNameTaken";
//            var model = gym == null ? string.Empty : gym.Id;
//            return PartialView(view, model);
//        }
