using System;
using System.Collections.Generic;
using System.Linq;

namespace AllStarScore.Admin.Models
{
    public class Schedule
    {
        public List<ScheduleDay> ScheduleDays { get; set; }
        public int DefaultDuration { get; set; }

        public Schedule(IEnumerable<DateTime> competitionDays)
        {
            ScheduleDays = competitionDays.Select(x => new ScheduleDay(x)).ToList();
            DefaultDuration = 15;
        }
        public class ScheduleDay
        {
            public DateTime Day { get; set; }
            public List<ScheduleItem> Items { get; set; }

            public ScheduleDay(DateTime day)
            {
                Day = day.AddHours(8);
                Items = new List<ScheduleItem>();
            }
        }

        public class ScheduleItem
        {
            public DateTime Time { get; set; }
            public int Index { get; set; }
            public int Duration { get; set; } //in minutes
        }
    }
}