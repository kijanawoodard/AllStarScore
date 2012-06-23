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
        public IJudgePanel Panel { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, IJudgePanel panel, ScoringMap scoringMap)
        {
            CompetitionId = performance.CompetitionId;
            Performance = performance;
            Panel = panel;
            ScoringMap = scoringMap;
        }
    }
}