﻿using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class ScoreSheetsController : RavenController
    {
        public ActionResult Details(string id)
        {
            var import =
                RavenSession
                    .Load<CompetitionInfo>(id);

            var model = new ScoreSheetsDetailsViewModel(import);
            return View(model);
        }
    }
}
