using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
        public Performance Performance { get; set; }
        public FiveJudgePanelPerformanceScoreCalculator Calculator { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, FiveJudgePanelPerformanceScoreCalculator calculator, ScoringMap scoringMap)
        {
            Performance = performance;
            Calculator = calculator;
            ScoringMap = scoringMap;
        }
    }
}