using System;
using System.Collections.Generic;

namespace AllStarScore.Models
{
    public class CompetitionImport
    {
        public string Id { get { return CompetitionId; } }

        public string CompanyName { get; set; }
        public string CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionDescription { get; set; }
        public List<DateTime> Days { get; set; } 
        public List<Division> Divisions { get; set; }
        public List<Level> Levels { get; set; }
        public List<Performance> Performances { get; set; }
    }
}