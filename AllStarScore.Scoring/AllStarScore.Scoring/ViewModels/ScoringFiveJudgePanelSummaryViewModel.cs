using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
        public string CompetitionId { get; set; }
        public Performance Performance { get; set; }
        public IJudgePanel Panel { get; set; }

        public ScoringFiveJudgePanelViewModel(Performance performance, IJudgePanel panel)
        {
			//CompetitionId = performance.CompetitionId;//TODO: MARK
            Performance = performance;
            Panel = panel;
        }
    }
}