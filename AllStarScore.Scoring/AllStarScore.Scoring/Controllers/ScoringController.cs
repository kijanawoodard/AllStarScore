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

            var model = new ScoringFiveJudgePanelViewModel(performance.Value, scores, new ScoringMap());
            
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
                    .Query<JudgeScore, JudgeScoreByPerformance>() //even though we have the id, i'm querying for the projection; if we drop history on JudgeScore due to a bundle, go back to Load
                    .Where(x => x.Id == request.CalculateJudgeScoreId())
                    .As<JudgeScoreByPerformance.Result>()
                    .FirstOrDefault();

            score = score ?? new JudgeScoreByPerformance.Result{PerformanceId = request.PerformanceId, JudgeId = request.JudgeId};

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
                            .Load<JudgeScore>(command.Id);

                    if (score.Id == null)
                    {
                        score = new JudgeScore(command.PerformanceId, command.JudgeId);
                        RavenSession.Store(score);
                    }

                    score.Update(command);

                    return new JsonDotNetResult(score);
                });
        }
    }
}
