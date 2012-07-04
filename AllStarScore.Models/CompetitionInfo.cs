using System;
using System.Collections.Generic;

namespace AllStarScore.Models
{
    public class CompetitionInfo
    {
        public string Id { get { return CompetitionId; } }

        public Company Company { get; set; }
        public Competition Competition { get; set; }
        public string CompanyName { get; set; } //TODO: remove these 4
        public string CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionDescription { get; set; }
        public List<DateTime> Days { get; set; } 
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<TeamRegistrationByCompetitionResults> Registrations { get; set; }
        public Schedule Schedule { get; set; }
        public List<Performance> Performances { get; set; } //TODO: remove
    }
}