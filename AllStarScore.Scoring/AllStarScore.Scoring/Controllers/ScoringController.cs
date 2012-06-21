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

            var calculator = GetCalculator(id);

            var model = new ScoringFiveJudgePanelViewModel(performance.Value, calculator, new ScoringMap());
            
            return View(model);
        }

        private FiveJudgePanelPerformanceScoreCalculator GetCalculator(string performanceId)
        {
            var scores =
                RavenSession
                    .Query<JudgeScore, JudgeScoreIndex>()
                    .Where(x => x.PerformanceId == performanceId)
                    .As<JudgeScoreIndex.Result>()
                    .ToList();

            var calculator = new FiveJudgePanelPerformanceScoreCalculator(scores);
            return calculator;
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
                    var calculator = GetCalculator(command.PerformanceId); //don't trust the client

                    var performance =
                        RavenSession
                            .Load<Performance>(command.PerformanceId);

                    //hmmm - shared model coming back to bite
                    performance.ScoringComplete = true;
                    performance.FinalScore = calculator.FinalScore;

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
