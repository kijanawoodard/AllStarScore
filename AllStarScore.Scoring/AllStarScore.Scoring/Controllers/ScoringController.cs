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
        public ActionResult FiveJudgePanel(string id)
        {
            var performance =
                RavenSession
                    .Advanced.Lazily
                    .Load<Performance>(id);

            var scores =
                RavenSession
                    .Query<JudgeScore, JudgeScoreByPerformance>()
                    .Where(x => x.PerformanceId == id)
                    .As<JudgeScoreByPerformance.Result>()
                    .ToList();

            var calculator = new FiveJudgePanelPerformanceScoreCalculator(scores);

            var model = new ScoringFiveJudgePanelViewModel(performance.Value, scores, calculator, new ScoringMap());
            
            return View(model);
        }

        [HttpGet]
        public ActionResult ScoreEntry(ScoreEntryRequestModel request)
        {
            var performance =
                RavenSession
                    .Load<Performance>(request.PerformanceId);

            var score =
                RavenSession
                    .Load<JudgeScore>(request.CalculateJudgeScoreId());

            score = score ?? new JudgeScore(request.PerformanceId, request.JudgeId);

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

                    if (score == null)
                    {
                        score = new JudgeScore(command.PerformanceId, command.JudgeId);
                        RavenSession.Store(score);
                    }

                    score.Update(command);

                    return new JsonDotNetResult(score);
                });
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
                    var performance =
                        RavenSession
                            .Load<Performance>(command.PerformanceId);

                    //hmmm - shared model coming back to bite
                    performance.ScoringComplete = true;

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

                    return new JsonDotNetResult(true);
                });
        }
    }
}
