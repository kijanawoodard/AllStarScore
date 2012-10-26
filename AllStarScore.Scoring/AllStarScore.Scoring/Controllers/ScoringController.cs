using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllStarScore.Extensions;
using AllStarScore.Library.RavenDB;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class ScoringController : RavenController
    {
        [HttpGet]
        public ActionResult Summary(string performanceId)
        {
            //HACK: pivot point established; can put panel type string on competition or performance as needed and follow the logic
            return FiveJudgePanelSummary(performanceId);
        }

        [HttpGet]
        public ActionResult FiveJudgePanelSummary(string performanceId)
        {
        	var scores = GetScores(performanceId);

            var panel = new FiveJudgePanel(scores);
			var score = GetPerformanceScore(performanceId);
            var model = new ScoringFiveJudgePanelViewModel(performanceId, score, panel);

            return View("FiveJudgePanelSummary", model);
        }

		[HttpGet]
		public JsonDotNetResult HighMediumLow(string performanceId)
		{
			var scores = GetScores(performanceId);
			var panel = new FiveJudgePanel(scores);
			var model = new HighMediumLowViewModel(panel);
			return new JsonDotNetResult(model);
		}

        private List<JudgeScore> GetScores(string performanceId)
        {
            var result =
                RavenSession
        			.LoadStartingWith<JudgeScore>(JudgeScore.FormatId(performanceId))
					.ToList();

            return result;
        }

		private PerformanceScore GetPerformanceScore(string performanceId)
		{
			var id = PerformanceScore.FormatId(performanceId);
			var result = RavenSession.Load<PerformanceScore>(id);

			return result ?? new PerformanceScore { PerformanceId = id };
		}

        [HttpGet]
        public ActionResult ScoreEntry(ScoreEntryRequestModel request)
        {
        	var ok = FiveJudgePanel.JudgeIds.Contains(request.JudgeId); //HACK: pivot if more panel types; move to attribute?; good spot for Fubu
        	ok = ok && User.CanActAsJudge(request.JudgeId);

            if (!ok)
				return RedirectToAction("Summary", "Scoring", new { PerformanceId = request.PerformanceId.ForScoringMvc() });

            var score =
                RavenSession
                    .Load<JudgeScore>(JudgeScore.FormatId(request.PerformanceId, request.JudgeId));

            if (score == null)
            {
				score = new JudgeScore(request.PerformanceId, request.JudgeId);//TODO: MARK
                RavenSession.Store(score);
            }

            var model = new ScoringScoreEntryViewModel(score);
            return View(model);
        }

        [HttpPost]
        public JsonDotNetResult ScoreEntry(ScoreEntryUpdateCommand command)
        {
            return Execute(
                action: () =>
                {
                    var ps = GetPerformanceScore(command.PerformanceId);

                    var result = "";
					if (ps.IsScoringComplete) //must have been sitting on the page; bail out and don't update the score
                    {
						result = Url.Action("Summary", "Scoring", new { performanceId = command.PerformanceId.ForScoringMvc() });
                    }
                    else
                    {
						var score =
						RavenSession
							.Load<JudgeScore>(JudgeScore.FormatId(command.PerformanceId, command.JudgeId));

                        score.Update(command);
                        result = GetNextScoreEntryUrl(command.PerformanceId, command.JudgeId);  
                    }
                    
                    return new JsonDotNetResult(result);
                });
        }

        private string GetNextScoreEntryUrl(string performanceId, string judgeId)
        {
            //TODO: switch on tabulator vs judge

            var judges = FiveJudgePanel.JudgeIds.ToList(); //HACK: pivot if more panel types

            var result = "";
            if (User.IsJudge() || judgeId.Equals(judges.Last(), StringComparison.InvariantCultureIgnoreCase))
            {
				result = Url.Action("Summary", "Scoring", new { performanceId = performanceId.ForScoringMvc() });
            }
            else
            {
                var nextIndex = judges.FindIndex(x => x == judgeId) + 1;
                var nextJudge = judges[nextIndex];
				result = Url.Action("ScoreEntry", "Scoring", new { performanceId = performanceId.ForScoringMvc(), judgeId = nextJudge });
            }

            return result;
        }

        [HttpPost]
        public JsonDotNetResult MarkTeamDidNotCompete(MarkTeamDidNotCompeteCommand command)
        {
            return Execute(
                action: () =>
                {
					var score = GetPerformanceScore(command.PerformanceId);
					score.Update(command);

                    return new JsonDotNetResult(true);
                });
        }

        [HttpPost]
        public JsonDotNetResult MarkTeamDidCompete(MarkTeamDidCompeteCommand command)
        {
            return Execute(
                action: () =>
                {
					var score = GetPerformanceScore(command.PerformanceId);
					score.Update(command);

                    return new JsonDotNetResult(false);
                });
        }

        [HttpPost]
        public JsonDotNetResult MarkTeamScoringComplete(MarkTeamScoringCompleteCommand command)
        {
            return Execute(
                action: () =>
                {
					var score = GetPerformanceScore(command.PerformanceId);
                	var scores = GetScores(command.PerformanceId);
                	var calculator = new FiveJudgePanelPerformanceScoreCalculator(scores);
                	
					score.Update(calculator);
					score.Update(command);
					RavenSession.Store(score);

                    return new JsonDotNetResult(true);
                });
        }

        [HttpPost]
        public JsonDotNetResult MarkTeamScoringOpen(MarkTeamScoringOpenCommand command)
        {
            return Execute(
                action: () =>
                {
					var score = GetPerformanceScore(command.PerformanceId);
					score.Update(command);

                    return new JsonDotNetResult(true);
                });
        }
    }
}
