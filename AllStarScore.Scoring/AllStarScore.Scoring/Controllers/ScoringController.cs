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
    public class ScoringController : RavenController
    {
        [HttpGet]
        public ActionResult FiveJudgePanel(string id)
        {
            var performance =
                RavenSession
                    .Load<Performance>(id);

            var model = new ScoringFiveJudgePanelViewModel(performance, new ScoringMap());
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
                    .Load<JudgeScore>(request.JudgeScoreId);

            score = score ?? new JudgeScore() {JudgeId = request.JudgeId};

            var model = new ScoringScoreEntryViewModel(performance, score, new ScoringMap());
            return View(model);
        }
    }
}
