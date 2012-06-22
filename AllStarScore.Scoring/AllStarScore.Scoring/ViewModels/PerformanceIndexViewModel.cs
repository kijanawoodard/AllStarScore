using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllStarScore.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class PerformanceIndexViewModel
    {
        public string CompetitionId { get; set; }
        public IEnumerable<Performance> Performances { get; set; }

        public PerformanceIndexViewModel(string competitionId, IEnumerable<Performance> performances)
        {
            CompetitionId = competitionId;
            Performances = performances;
        }
    }
}