using System;
using System.Collections.Generic;
using System.Linq;

namespace AllStarScore.Admin.Models
{
    public class Schedule
    {
        public List<ScheduleDay> Days { get; set; }
        public int DefaultDuration { get; set; }
        public int NumberOfPanels { get; set; }

        public Schedule(IEnumerable<DateTime> competitionDays)
        {
            Days = competitionDays.Select(x => new ScheduleDay(x)).ToList();
            DefaultDuration = 15;
            NumberOfPanels = 2;
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
        }
    }
}