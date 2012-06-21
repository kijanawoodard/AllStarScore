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
        public AverageScoreReporting AverageReporting { get; set; }

        public ReportingSinglePerformanceViewModel(string competitionId, TeamScoreReporting reporting, AverageScoreReporting averageReporting)
        {
            CompetitionId = competitionId;
            Reporting = reporting;
            AverageReporting = averageReporting;
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

    public class ReportingAveragesViewModel
    {
        public AverageScoreReporting Reporting { get; set; }

        public ReportingAveragesViewModel(AverageScoreReporting reporting)
        {
            Reporting = reporting;
        }
    }
}