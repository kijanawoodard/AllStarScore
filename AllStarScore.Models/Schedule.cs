using System;
using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models.Commands;
using Newtonsoft.Json;

namespace AllStarScore.Models
{
    public class Schedule
    {
        public string Id { get; set; }

        public string CompetitionId { get; set; }

        public int DefaultDuration { get; set; } //in minutes
        public int DefaultWarmupTime { get; set; } //in minutes
        public int NumberOfPanels { get; set; }
        public int NumberOfPerformances { get; set; }
        public List<ScheduleDay> Days { get; set; } 

        [JsonIgnore]
        public IEnumerable<PerformanceEntry> PerformanceEntries
        {
            get
            {
                return Days
                    .SelectMany(day => day.Entries)
                    .Where(entry => entry.ContainsKey("registrationId"))
                    .Select(entry => new PerformanceEntry(entry));
            }
        }

        public Schedule()
        {
            DefaultDuration = 3;
            DefaultWarmupTime = 40;
            NumberOfPanels = 2;
            NumberOfPerformances = 1;
            Days = new List<ScheduleDay>();
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

        public class PerformanceEntry
        {
            public string RegistrationId { get; set; }
            public string Panel { get; set; }
            public DateTime PerformanceTime { get; set; }
            public int WarmupTime { get; set; }
            public int Duration { get; set; }
            public int Index { get; set; }
            public string Template { get; set; }

            public PerformanceEntry(Dictionary<string, string> entry)
            {
                RegistrationId = entry["registrationId"];
                Panel = entry["panel"];
                PerformanceTime = DateTime.Parse(entry["time"]);
                WarmupTime = int.Parse(entry["warmupTime"]);
                Duration = int.Parse(entry["duration"]);
                Index = int.Parse(entry["index"]);
                Template = entry["template"];
            }
        }
    }
}