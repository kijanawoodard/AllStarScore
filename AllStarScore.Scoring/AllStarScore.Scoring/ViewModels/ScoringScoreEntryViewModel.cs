using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Controllers;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringScoreEntryViewModel
    {
        public Performance Performance { get; set; }
        public JudgeScore Score { get; set; }
        public Dictionary<string, string> ScoringMap { get; set; }

        public ScoringScoreEntryViewModel(Performance performance, JudgeScore score, ScoringMap scoringMap)
        {
            Performance = performance;
            Score = score;
            ScoringMap = scoringMap.All;
        }
    }

    public class ScoreEntryRequestModel
    {
        public string PerformanceId { get; set; }
        public string JudgeId { get; set; }
        public string JudgeScoreId { get { return PerformanceId + "/scores-" + JudgeId; } }
    }
}