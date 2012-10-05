using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;
using AllStarScore.Scoring.ViewModels;
using AllStarScore.Library.RavenDB;

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
        			.LoadStartingWith<PerformanceScore>(id + "/scores/");

			var info =
				RavenSession
					.Load<CompetitionInfo>(id);

            var calculator = new SmallGymRankingCalculator(); //TODO: indirect
            var generator = new TeamScoreGenerator();
            var scores = generator.From(performances, info);
            var reporting = new TeamScoreReporting(scores);
            reporting.Rank(calculator);

            var model = new ReportingSinglePerformanceViewModel(id, reporting);
            return View(model);
        }

        public ActionResult TwoPerformance(string id)
        {
			var performances =
				RavenSession
					.LoadStartingWith<PerformanceScore>(id + "/scores/");

			var info =
				RavenSession
					.Load<CompetitionInfo>(id);

            var calculator = new SmallGymRankingCalculator(); //TODO: indirect
            var generator = new TeamScoreGenerator();
            var scores = generator.From(performances, info).Where(x => x.PerformanceScores.Count == 2);
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
					.LoadStartingWith<PerformanceScore>(id + "/scores/");

            var averages = new AverageScoreReporting(performances);

            var model = new ReportingAveragesViewModel(averages);
            return PartialView(model);
        }
    }
}
