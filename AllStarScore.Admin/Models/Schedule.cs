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
            Days = new List<ScheduleDay>();
            DivisionPanels = new List<DivisionPanelAssignments>();
            
            History = new Collection<ICommand>();
        }

        public Schedule(Competition competition) : this()
        {
            CompetitionId = competition.Id;
            Days = competition
                        .FirstDay
                        .GetDateRange(competition.LastDay)
                        .Select(x => new ScheduleDay(x.AddHours(8))) //start at 8 am
                        .ToList();   
        }

        public void Update(SchedulingEditCommand command)
        {
            DefaultDuration = command.DefaultDuration;
            DefaultWarmupTime = command.DefaultWarmupTime;
            NumberOfPanels = command.NumberOfPanels;
            DivisionPanels = command.DivisionPanels;

            History.Add(command);
        }

        public class ScheduleDay
        {
            public ScheduleDay()
            {
                Entries = new List<ScheduleEntry>();
            }

            public ScheduleDay(DateTime day) : this()
            {
                Day = day;
            }

            public DateTime Day { get; set; }
            public List<ScheduleEntry> Entries { get; set; }
        }

        public class ScheduleEntry
        {
            public string RegistrationId { get; set; }
            public DateTime Time { get; set; }
            public int Index { get; set; }
            public int Duration { get; set; } //in minutes
            public int WarmupTime { get; set; } //in minutes
            public string Panel { get; set; }
            public string Template { get; set; }
        }

        public class DivisionPanelAssignments
        {
            public string DivisionId { get; set; }
            public string Panel { get; set; }
        }
    }
}