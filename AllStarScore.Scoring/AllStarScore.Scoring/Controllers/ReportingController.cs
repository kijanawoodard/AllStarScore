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
                    .Where(x => x.CompetitionId == id)
                    .Take(int.MaxValue)
                    .ToList();

            var calculator = new SmallGymRankingCalculator(); //TODO: indirect
            var generator = new TeamScoreGenerator();
            var scores = generator.From(performances);
            var reporting = new TeamScoreReporting(scores);
            reporting.Rank(calculator);

            var model = new ReportingSinglePerformanceViewModel(id, reporting);
            return View(model);
        }

        public ActionResult TwoPerformance(string id)
        {
            var performances =
                RavenSession
                    .Query<Performance>()
                    .Take(int.MaxValue)
                    .ToList();

            var calculator = new SmallGymRankingCalculator(); //TODO: indirect
            var generator = new TeamScoreGenerator();
            var scores = generator.From(performances).Where(x => x.PerformanceScores.Count == 2);
            var reporting = new TeamScoreReporting(scores);
            reporting.Rank(calculator);

            var model = new ReportingTwoPerformanceViewModel(id, reporting);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Averages(string id)
        {
            var performances =
                RavenSession
                    .Query<Performance>()
                    .Where(x => x.CompetitionId == id)
                    .Take(int.MaxValue)
                    .ToList();

            var scores =
                RavenSession
                    .Query<JudgeScore>()
                    .Take(int.MaxValue)
                    .ToList();

            var averages = new AverageScoreReporting(performances, scores);

            var model = new ReportingAveragesViewModel(averages);
            return PartialView(model);
        }
    }
}
