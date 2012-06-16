using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
        public Performance Performance { get; set; }
        public IEnumerable<JudgeScoreByPerformance.Result> Scores { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, IEnumerable<JudgeScoreByPerformance.Result> scores, ScoringMap scoringMap)
        {
            Performance = performance;
            Scores = scores;
            ScoringMap = scoringMap;
        }
    }
}