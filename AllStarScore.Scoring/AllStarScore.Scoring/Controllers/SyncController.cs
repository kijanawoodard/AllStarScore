using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Extensions;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;
using AllStarScore.Scoring.ViewModels;
using Newtonsoft.Json;

namespace AllStarScore.Scoring.Controllers
{
    public class SyncController : RavenController
    {
    	public static readonly string TenantName = "local";

        public ActionResult Import(string id)
        {
			var security =
				RavenSession
					.Load<Synchronization>(Synchronization.FormatId(TenantName));

			var url = string.Format("{0}{1}/competitions_{2}", security.Url, security.Token, id);

            var client = new WebClient();
            var data = client.DownloadString(url);
            var model = JsonConvert.DeserializeObject<CompetitionInfo>(data);

            RavenSession.Store(model);
        	Populate(model);

			return RedirectToAction("Index", "Performance", new { id = model.Id.ForScoringMvc() });
        }

		void Populate(CompetitionInfo info)
		{
			var performances =
				info.Registrations
					.Select(x => x.GetPerformances(info.Competition))
					.SelectMany(x => x)
					.ToList();
		
			var panel = new FiveJudgePanel(new List<JudgeScore>());
			performances.ForEach(performance =>
			{
				var scores = panel.Judges.Select(judge => new JudgeScore(performance.Id, judge.Id)).ToList();
				var calculator = new FiveJudgePanelPerformanceScoreCalculator(scores);
		
				var score = new PerformanceScore {PerformanceId = performance.Id};
				var command = new MarkTeamScoringCompleteCommand
				{
					PerformanceId = performance.Id,
					RegistrationId = performance.RegistrationId,
					DivisionId = performance.DivisionId,
					CommandByUser = "system",
					CommandWhen = DateTime.UtcNow
				};
		
				score.Update(calculator);
				score.Update(command);
				score.IsScoringComplete = false;

				scores.ForEach(RavenSession.Store);
				RavenSession.Store(score);
			});
		}

//        public void UpdatePerformances(CompetitionInfo model)
//        {
//            var performances =
//                RavenSession
//                    .Query<Performance>()
//                    .Where(x => x.CompetitionId == model.CompetitionId)
//                    .Take(int.MaxValue) //not expecting more than 100s, but likely slighly more than 128
//                    .ToList();
//
//            var scores =
//                RavenSession
//                    .Query<JudgeScore>()
//                    .Where(x => x.CompetitionId == model.CompetitionId)
//                    .Take(int.MaxValue) //TODO: page through these, could be more than 1024
//                    .ToList();
//
//            foreach (var performance in performances)
//            {
//                RavenSession.Delete(performance);
//            }
//
//            foreach (var score in scores)
//            {
//                RavenSession.Delete(score);
//            }
//
//            foreach (var performance in model.Performances)
//            {
//                RavenSession.Store(performance);
//            }
//        }

		public ActionResult Init(string id)
		{
			var doc =
				RavenSession
					.Load<Synchronization>(Synchronization.FormatId(TenantName));

			if (doc == null && id != null)
			{
				doc = new Synchronization
				      {
				      	CompanyId = TenantName,
				      	Token = id
				      };
				RavenSession.Store(doc);
			}

			return new JsonDotNetResult("ok");
		}
    }
}
