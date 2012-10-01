using AllStarScore.Extensions;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringFiveJudgePanelViewModel
    {
		public string CompetitionId { get; set; }
    	public string PerformanceId { get; set; }
    	public PerformanceScore Score { get; set; }
    	public IJudgePanel Panel { get; set; }

        public ScoringFiveJudgePanelViewModel(string performanceId, PerformanceScore score, IJudgePanel panel)
        {
        	CompetitionId = performanceId.ExtractCompetitionId();
        	PerformanceId = performanceId;
        	Score = score;
        	Panel = panel;
        }
    }
}