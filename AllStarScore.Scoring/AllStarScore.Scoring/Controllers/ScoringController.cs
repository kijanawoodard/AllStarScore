using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Extensions;
using AllStarScore.Library.RavenDB;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;
using AllStarScore.Scoring.ViewModels;
using Raven.Client.Linq;

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
            var model = new ScoringFiveJudgePanelViewModel(performanceId, panel);

            return View("FiveJudgePanelSummary", model);
        }

        private List<JudgeScore> GetScores(string performanceId)
        {
            var result =
                RavenSession
        			.LoadStartingWith<JudgeScore>(JudgeScore.FormatId(performanceId))
					.ToList();

            return result;
        }

        [HttpGet]
        public ActionResult ScoreEntry(ScoreEntryRequestModel request)
        {
            if (!FiveJudgePanel.JudgeIds.Contains(request.JudgeId)) //HACK: pivot if more panel types; move to attribute?; good spot for Fubu
                return RedirectToAction("Summary", "Scoring", new {request.PerformanceId});

            var score =
                RavenSession
                    .Include<JudgeScore>(x => x.PerformanceId)
                    .Load<JudgeScore>(JudgeScore.FormatId(request.PerformanceId, request.JudgeId));

            var performance =
                RavenSession
                    .Load<Performance>(request.PerformanceId);

            if (score == null)
            {
				score = new JudgeScore(request.PerformanceId, request.JudgeId);//TODO: MARK
                RavenSession.Store(score);
            }

            var model = new ScoringScoreEntryViewModel(performance, score);
            return View(model);
        }

        [HttpPost]
        public JsonDotNetResult ScoreEntry(ScoreEntryUpdateCommand command)
        {
            return Execute(
                action: () =>
                {
                    var score =
                        RavenSession
                            .Include<JudgeScore>(x => x.PerformanceId)
							.Load<JudgeScore>(JudgeScore.FormatId(command.PerformanceId, command.JudgeId));

                    var performance =
                        RavenSession
                            .Load<Performance>(command.PerformanceId);

                    var result = "";
					if (true) //TODO: MARK (performance.ScoringComplete) //must have been sitting on the page; bail out and don't update the score
                    {
						result = Url.Action("Summary", "Scoring", new { performanceId = command.PerformanceId.ForScoringMvc() });
                    }
                    else
                    {
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
            if (judgeId.Equals(judges.Last(), StringComparison.InvariantCultureIgnoreCase))
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
                    var performance =
                        RavenSession
                            .Load<Performance>(command.PerformanceId);

                    //hmmm - shared model coming back to bite
					//TODO: MARK performance.DidNotCompete = true;

                    return new JsonDotNetResult(true);
                });
        }

        [HttpPost]
        public JsonDotNetResult MarkTeamDidCompete(MarkTeamDidCompeteCommand command)
        {
            return Execute(
                action: () =>
                {
                    var performance =
                        RavenSession
                            .Load<Performance>(command.PerformanceId);

                    //hmmm - shared model coming back to bite
					//TODO: MARK performance.DidNotCompete = false;

                    return new JsonDotNetResult(false);
                });
        }

        [HttpPost]
        public JsonDotNetResult MarkTeamScoringComplete(MarkTeamScoringCompleteCommand command)
        {
            return Execute(
                action: () =>
                {
                    var scores = GetScores(command.PerformanceId); //don't trust the client
                    var panel = new FiveJudgePanel(scores);

                    var performance =
                        RavenSession
                            .Load<Performance>(command.PerformanceId);

                    //hmmm - shared model coming back to bite
					//TODO: MARK

//                    performance.ScoringComplete = true;
//                    performance.FinalScore = panel.Calculator.FinalScore;

                    return new JsonDotNetResult(true);
                });
        }

        [HttpPost]
        public JsonDotNetResult MarkTeamScoringOpen(MarkTeamScoringOpenCommand command)
        {
            return Execute(
                action: () =>
                {
                    var performance =
                        RavenSession
                            .Load<Performance>(command.PerformanceId);

                    //hmmm - shared model coming back to bite
					//TODO: MARK
//                    performance.ScoringComplete = false;
//                    performance.FinalScore = 0;

                    return new JsonDotNetResult(true);
                });
        }
    }
}
