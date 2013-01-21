using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
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
        			.LoadStartingWith<PerformanceScore>(id + "/scores/")
        			.Where(x => x.IsFirstPerformance);
					
			var info =
				RavenSession
					.Load<CompetitionInfo>(id);

            var generator = new TeamScoreGenerator();
            var scores = generator.From(performances, info);
            var reporting = new TeamScoreReporting(scores);
            reporting.Rank();

            var model = new ReportingSinglePerformanceViewModel(id, reporting);
            return View(model);
        }

        public ActionResult TwoPerformance(string id)
        {
			var performances =
				RavenSession
					.LoadStartingWith<PerformanceScore>(id + "/scores/")
					.ToList();

			var info =
				RavenSession
					.Load<CompetitionInfo>(id);

        	var registrations =
				info.Registrations
					.Where(x => x.GetPerformances(info.Competition).Count() == 2)
					.Select(x => x.Id)
					.ToList();

			performances = performances.Where(x => registrations.Contains(x.RegistrationId)).ToList();
        	var firsts = performances.Where(x => x.IsFirstPerformance);
        	var seconds = performances.Where(x => x.IsSecondPerformance);

            var generator = new TeamScoreGenerator();
        	var scores = generator.From(firsts, info).ToList();
			scores.ForEach(x =>
			{
				x.FirstScorePercentage = info.FirstPerformancePercentage;

				var second = seconds.FirstOrDefault(s => s.RegistrationId == x.RegistrationId);
				var total = 0.0M;
				if (second != null)
				{
					//TODO: for worlds, switch division/level to "Worlds"
					total = second.TotalScore;
				}

				x.PerformanceScores.Add(total);
			});

            var reporting = new TeamScoreReporting(scores);
            reporting.Rank();

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
