using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
        public Performance Performance { get; set; }
        public IEnumerable<JudgeScoreByPerformance.Result> Judges { get; set; }
        public FiveJudgePanelPerformanceScoreCalculator Calculator { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, IEnumerable<JudgeScoreByPerformance.Result> scores, FiveJudgePanelPerformanceScoreCalculator calculator, ScoringMap scoringMap)
        {
            Performance = performance;
            Judges = scores;
            Calculator = calculator;
            ScoringMap = scoringMap;
        }
    }
}