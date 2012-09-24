using System;
using System.Collections.Generic;

namespace AllStarScore.Models.Commands
{
    public class SchedulingEditCommand : ICommand, ICompanyCommand
    {
        public string Id { get; set; }

        public int DefaultDuration { get; set; } //in minutes
        public int DefaultWarmupTime { get; set; } //in minutes
		public int DefaultBreakDuration { get; set; } //in minutes
		public int DefaultAwardsDuration { get; set; } //in minutes
        public int NumberOfPanels { get; set; }
        public List<Schedule.ScheduleDay> Days { get; set; }
        public Dictionary<string, string> DivisionPanels { get; set; }

        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }

    public class ScheduleCreateCommand : ICommand, ICompanyCommand, IBelongToCompetition
    {
        public string CompetitionId { get; set; }
        public IEnumerable<DateTime> Days { get; set; } 

        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}