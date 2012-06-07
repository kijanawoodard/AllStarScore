using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.Infrastructure.Utilities;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Models
{
    public class Schedule
    {
        public string Id { get; set; }

        public string CompetitionId { get; set; }

        public int DefaultDuration { get; set; } //in minutes
        public int DefaultWarmupTime { get; set; } //in minutes
        public int NumberOfPanels { get; set; }
        public List<ScheduleDay> Days { get; set; }
        public List<DivisionPanelAssignments> DivisionPanels { get; set; }

        public ICollection<ICommand> History { get; private set; }

        public Schedule()
        {
            DefaultDuration = 3;
            DefaultWarmupTime = 40;
            NumberOfPanels = 2;
            DivisionPanels = new List<DivisionPanelAssignments>();
            History = new Collection<ICommand>();
        }

        public Schedule(Competition competition) : this()
        {
            CompetitionId = competition.Id;
            Days = competition
                        .FirstDay
                        .GetDateRange(competition.LastDay)
                        .Select(x => new ScheduleDay(x)).ToList();   
        }

        public void Update(SchedulingEditCommand command)
        {
            DefaultDuration = command.Schedule.DefaultDuration;
            DefaultWarmupTime = command.Schedule.DefaultWarmupTime;
            NumberOfPanels = command.Schedule.NumberOfPanels;
            Days = command.Schedule.Days;
            DivisionPanels = command.Schedule.DivisionPanels;

            command.Schedule = null; //blank out history; this data will probably thrash a lot without meaning. When we publish for the competitions is significant.
            History.Add(command);
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