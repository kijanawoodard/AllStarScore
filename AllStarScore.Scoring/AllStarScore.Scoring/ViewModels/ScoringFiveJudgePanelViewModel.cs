using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Controllers;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
        public Performance Performance { get; set; }
        public Dictionary<string, string> ScoringMap { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, ScoringMap scoringMap)
        {
            Performance = performance;
            ScoringMap = scoringMap.All;
        }
    }
}