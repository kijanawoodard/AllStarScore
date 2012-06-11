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
        public Dictionary<string, string> DivisionPanels { get; set; }
        
        public Schedule()
        {
            DefaultDuration = 3;
            DefaultWarmupTime = 40;
            NumberOfPanels = 2;
            Days = new List<ScheduleDay>();
            DivisionPanels = new Dictionary<string, string>();
        }

        public Schedule(Competition competition) : this()
        {
            CompetitionId = competition.Id;
            Days = competition
                        .Days
                        .Select(x => new ScheduleDay(x.AddHours(8))) //start at 8 am
                        .ToList();   
        }

        public void Update(SchedulingEditCommand command)
        {
            DefaultDuration = command.DefaultDuration;
            DefaultWarmupTime = command.DefaultWarmupTime;
            NumberOfPanels = command.NumberOfPanels;
            DivisionPanels = command.DivisionPanels;
            Days = command.Days;
        }

        public class ScheduleDay
        {
            public ScheduleDay()
            {
                Entries = new List<Dictionary<string, string>>();
            }

            public ScheduleDay(DateTime day) : this()
            {
                Day = day;
            }

            public DateTime Day { get; set; }
            public List<Dictionary<string, string>> Entries { get; set; }
        }
    }
}