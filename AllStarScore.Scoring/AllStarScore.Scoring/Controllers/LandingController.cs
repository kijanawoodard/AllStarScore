using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class LandingController : RavenController
    {
        public ActionResult Index()
        {
            var competitions =
                RavenSession
                    .Query<CompetitionInfo>()
                    .Take(int.MaxValue) //shouldn't be too many of these; TODO: look into expiring competitions from scoring laptop
                    .ToList();

            var model = new LandingIndexViewModel(competitions);
            return View(model);
        }
    }
}
