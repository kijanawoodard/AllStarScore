using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;
using AllStarScore.Library.RavenDB;

namespace AllStarScore.Admin.Controllers
{
    public class LevelController : RavenController
    {
        public ActionResult Index()
        {
            var levels =
                RavenSession
                    .LoadStartingWith<Level>(Level.FormatId(CurrentCompanyId))
                    .ToList();

            var model = new LevelIndexViewModel(levels);
            return View(model);
        }

        public ActionResult Details(string id)
        {
            var level =
                RavenSession
                    .Load<Level>(id);

            var divisions =
                RavenSession
                    .LoadStartingWith<Division>(Division.FormatId(CurrentCompanyId, id))
                    .ToList();

            var model = new LevelDetailsViewModel(level, divisions);
            return View(model);
        }

//        public string All()
//        {
//            var divisions =
//                RavenSession
//                    .Query<Division>()
//                    .ToList();
//
//            return Json(divisions, JsonRequestBehavior.AllowGet).Data as string;
//        }

    }
}
