using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            var performance =
                RavenSession
                    .Load<Performance>(performanceId);
            
            var panel = new FiveJudgePanel(scores);
            var model = new ScoringFiveJudgePanelViewModel(performance, panel, new ScoringMap());

            return View("FiveJudgePanelSummary", model);
        }

        private List<JudgeScoreIndex.Result> GetScores(string performanceId)
        {
            var result =
                RavenSession
                    .Query<JudgeScore, JudgeScoreIndex>()
                    .Customize(x => x.Include<JudgeScoreIndex.Result>(r => r.PerformanceId))
                    .Where(x => x.PerformanceId == performanceId)
                    .As<JudgeScoreIndex.Result>()
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
                    .Load<JudgeScore>(request.CalculateJudgeScoreId());

            var performance =
                RavenSession
                    .Load<Performance>(request.PerformanceId);

            if (score == null)
            {
                score = new JudgeScore(performance.CompetitionId, request.PerformanceId, request.JudgeId);
                RavenSession.Store(score);
            }

            var model = new ScoringScoreEntryViewModel(performance, score, new ScoringMap());
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
                            .Load<JudgeScore>(command.CalculateJudgeScoreId());

                    score.Update(command);

                    var result = GetNextScoreEntryUrl(command.PerformanceId, command.JudgeId);
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
                result = Url.Action("Summary", "Scoring", new { performanceId });
            }
            else
            {
                var nextIndex = judges.FindIndex(x => x == judgeId) + 1;
                var nextJudge = judges[nextIndex];
                result = Url.Action("ScoreEntry", "Scoring", new { performanceId, judgeId = nextJudge});
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
                    performance.DidNotCompete = true;

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
                    performance.DidNotCompete = false;

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
                    performance.ScoringComplete = true;
                    performance.FinalScore = panel.Calculator.FinalScore;

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
                    performance.ScoringComplete = false;
                    performance.FinalScore = 0;

                    return new JsonDotNetResult(true);
                });
        }
    }
}
