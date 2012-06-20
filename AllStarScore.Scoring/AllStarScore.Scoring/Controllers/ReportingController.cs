using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class ReportingController : RavenController
    {
        public ActionResult Index(string id)
        {
            var model = new ReportingIndexViewModel(id);
            return View(model);
        }

        public ActionResult SinglePerformance(string id)
        {
            var performances =
                RavenSession
                    .Query<Performance>()
                    .Take(int.MaxValue)
                    .ToList();

            var calculator = new SmallGymRankingCalculator(); //tODO: indirect
            var generator = new TeamScoreGenerator();
            var scores = generator.From(performances);
            scores.Rank(calculator);

            var model = new ReportingSinglePerformanceViewModel(id, scores);
            return View(model);
        }

    }
}
