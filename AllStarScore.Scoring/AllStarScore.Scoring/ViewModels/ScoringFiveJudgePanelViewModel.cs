using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
        public string CompetitionId { get; set; }
        public Performance Performance { get; set; }
        public FiveJudgePanelPerformanceScoreCalculator Calculator { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, FiveJudgePanelPerformanceScoreCalculator calculator, ScoringMap scoringMap)
        {
            CompetitionId = performance.CompetitionId;
            Performance = performance;
            Calculator = calculator;
            ScoringMap = scoringMap;
        }
    }
}