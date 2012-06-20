using System.Collections.Generic;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ReportingIndexViewModel
    {
        public string CompetitionId { get; set; }

        public ReportingIndexViewModel(string competitionId)
        {
            CompetitionId = competitionId;
        }
    }

    public class ReportingSinglePerformanceViewModel
    {
        public string CompetitionId { get; set; }
        public TeamScoreReporting Reporting { get; set; }

        public ReportingSinglePerformanceViewModel(string competitionId, TeamScoreReporting reporting)
        {
            CompetitionId = competitionId;
            Reporting = reporting;
        }
    }

    public class ReportingTwoPerformanceViewModel
    {
        public string CompetitionId { get; set; }
        public TeamScoreReporting Reporting { get; set; }

        public ReportingTwoPerformanceViewModel(string competitionId, TeamScoreReporting reporting)
        {
            CompetitionId = competitionId;
            Reporting = reporting;
        }
    }
}