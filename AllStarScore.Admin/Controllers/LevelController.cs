using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Models;
using AllStarScore.Library.RavenDB;
using AllStarScore.Models.Commands;

namespace AllStarScore.Admin.Controllers
{
    public class LevelController : RavenController
    {
        public ActionResult Index()
        {
            var competitionDivisions =
                RavenSession
					.Load<CompetitionDivisions>(CompetitionDivisions.FormatId(CurrentCompanyId));

            var model = new LevelIndexViewModel(competitionDivisions);
            return View(model);
        }

//        public ActionResult Details(string id)
//        {
//            var level =
//                RavenSession
//                    .Load<Level>(id);
//
//            var divisions =
//                RavenSession
//                    .LoadStartingWith<Division>(Division.FormatId(CurrentCompanyId, id))
//                    .ToList();
//
//            var model = new LevelDetailsViewModel(level, divisions);
//            return View(model);
//        }
//
//		[HttpPost]
//		public JsonDotNetResult Details(DivisionCreateCommand command)
//		{
//			return Execute(
//				action: () =>
//				{
//					var division = new Division();
//					division.Update(command);
//					RavenSession.Store(division);
//					RavenSession.SaveChanges();
//
//					return new JsonDotNetResult(division);
//				});
//		}
//
//		
//    }
//
//	public class DivisionController : RavenController
//	{
//		public JsonDotNetResult Edit(DivisionEditCommand command)
//		{
//			return Execute(
//				action: () =>
//				{
//					var division =
//						RavenSession.Load<Division>(command.Id);
//
//					division.Update(command);
//					RavenSession.Store(division);
//					RavenSession.SaveChanges();
//
//					return new JsonDotNetResult(division);
//				});
//		}
	}
}
