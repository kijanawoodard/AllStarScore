using System;
using System.Collections.Generic;

namespace AllStarScore.Models.Commands
{
    public class SchedulingEditCommand : ICommand
    {
        public string Id { get; set; }

        public int DefaultDuration { get; set; } //in minutes
        public int DefaultWarmupTime { get; set; } //in minutes
        public int NumberOfPanels { get; set; }
        public List<Schedule.ScheduleDay> Days { get; set; }
        public Dictionary<string, string> DivisionPanels { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}