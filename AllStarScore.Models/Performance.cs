using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllStarScore.Models
{
    public class Performance
    {
        public string Id { get; set; }

        public string RegistrationId { get; set; }

        public string CompetitionId { get; set; }
        public string GymId { get; set; }
        public string DivisionId { get; set; }
        public string LevelId { get; set; }

        public DateTime WarmupTime { get; set; }
        public DateTime PerformanceTime { get; set; }
        public string Panel { get; set; }

        public string GymName { get; set; }
        public string TeamName { get; set; }
        public string DivisionName { get; set; }
        public string LevelName { get; set; }

        public bool IsSmallGym { get; set; }
        public bool IsShowTeam { get; set; }
        public string GymLocation { get; set; }
        public int ParticipantCount { get; set; }
    }

    public class ScoringImportData
    {
        public string CompanyName { get; set; }
        public string CompetitionDescription { get; set; }
        public List<Performance> Performances { get; set; } 
    }
}
