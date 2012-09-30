using System;
using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Commands;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoringScoreEntryViewModel
    {
        public string CompetitionId { get; set; }
        public Performance Performance { get; set; }
        public JudgeScore Score { get; set; }

        public ScoringScoreEntryViewModel(Performance performance, JudgeScore score)
        {
//            CompetitionId = performance.CompetitionId;  //TODO: MARK
            Performance = performance;
            Score = score;
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
}