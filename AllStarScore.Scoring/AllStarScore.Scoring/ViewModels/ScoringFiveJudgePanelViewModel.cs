using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
        public Performance Performance { get; set; }
        public Dictionary<string, string> ScoringMap { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, ScoringMap scoringMap)
        {
            Performance = performance;
            ScoringMap = scoringMap.Templates;
        }
    }
}