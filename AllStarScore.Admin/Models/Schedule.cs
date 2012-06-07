using System;
using System.Collections.Generic;
using System.Linq;

namespace AllStarScore.Admin.Models
{
    public class Schedule
    {
        public int DefaultDuration { get; set; } //in minutes
        public int DefaultWarmupTime { get; set; } //in minutes
        public int NumberOfPanels { get; set; }
        public List<ScheduleDay> Days { get; set; }
        public List<DivisionPanelAssignments> DivisionPanels { get; set; }
        
        public Schedule(IEnumerable<DateTime> competitionDays)
        {
            DefaultDuration = 3;
            DefaultWarmupTime = 40;
            NumberOfPanels = 2;
            Days = competitionDays.Select(x => new ScheduleDay(x)).ToList();
            DivisionPanels = new List<DivisionPanelAssignments>();
        }

        public class ScheduleDay
        {
            public DateTime Day { get; set; }
            public List<ScheduleEntries> Entries { get; set; }

            public ScheduleDay(DateTime day)
            {
                Day = day.AddHours(8);
                Entries = new List<ScheduleEntries>();
            }
        }

        public class ScheduleEntries
        {
            public string RegistrationId { get; set; }
            public DateTime Time { get; set; }
            public int Index { get; set; }
            public int Duration { get; set; } //in minutes
            public int WarmupTime { get; set; } //in minutes
            public string Panel { get; set; }
        }

        public class DivisionPanelAssignments
        {
            public string DivisionId { get; set; }
            public string Panel { get; set; }
        }
    }
}