using System;
using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditCommand : ICommand
    {
        public string Id { get; set; }

        public int DefaultDuration { get; set; } //in minutes
        public int DefaultWarmupTime { get; set; } //in minutes
        public int NumberOfPanels { get; set; }
        public List<DateTime> Days { get; set; }
        public List<Schedule.DivisionPanelAssignments> DivisionPanels { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}