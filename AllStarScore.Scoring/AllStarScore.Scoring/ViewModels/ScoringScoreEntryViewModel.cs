using System;
using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Controllers;
using AllStarScore.Scoring.Infrastructure.Commands;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringScoreEntryViewModel
    {
        public Performance Performance { get; set; }
        public JudgeScore Score { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public ScoringScoreEntryViewModel(Performance performance, JudgeScore score, ScoringMap scoringMap)
        {
            Performance = performance;
            Score = score;
            ScoringMap = scoringMap;
        }
    }

    public class ScoreEntryRequestModel : IJudgeScoreId
    {
        public string PerformanceId { get; set; }
        public string JudgeId { get; set; }
    }

    public class ScoreEntryUpdateCommand : ICommand, IJudgeScoreId
    {
        public string Id { get; set; }
        public string PerformanceId { get; set; }
        public string JudgeId { get; set; }

        public Dictionary<string, ScoreEntry> Scores { get; set; }
        public decimal GrandTotal { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }

        public ScoreEntryUpdateCommand()
        {
            Scores = new Dictionary<string, ScoreEntry>();
        }
    }

    public class MarkTeamDidNotCompeteCommand : ICommand
    {
        public string PerformanceId { get; set; }
    
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }

    public class MarkTeamDidCompeteCommand : ICommand
    {
        public string PerformanceId { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }

    public class MarkTeamScoringCompleteCommand : ICommand
    {
        public string PerformanceId { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }

    public class MarkTeamScoringOpenCommand : ICommand
    {
        public string PerformanceId { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }

    public interface IJudgeScoreId
    {
        string PerformanceId { get; set; }
        string JudgeId { get; set; }
    }

    public static class ScoreExtensions
    {
        public static string CalculateJudgeScoreId(this IJudgeScoreId score)
        {
            return score.PerformanceId + "/scores-" + score.JudgeId;
        }
    }
}