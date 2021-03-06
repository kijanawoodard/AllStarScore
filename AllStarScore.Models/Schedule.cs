using System;
using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models.Commands;
using Newtonsoft.Json;

namespace AllStarScore.Models
{
    public class Schedule : ICanBeUpdatedByCommand, IBelongToCompany, IBelongToCompetition, IGenerateMyId
    {
        public string Id { get; set; }
        public int DefaultDuration { get; set; } //in minutes
        public int DefaultWarmupTime { get; set; } //in minutes
		public int DefaultBreakDuration { get; set; } //in minutes
		public int DefaultAwardsDuration { get; set; } //in minutes
        public int NumberOfPanels { get; set; }
        public List<ScheduleDay> Days { get; set; }
		public Dictionary<string, string> DivisionPanels { get; set; }

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

        public string CompetitionId { get; set; }
        public string CompanyId { get; set; }
        public string LastCommand { get; set; }
        public string LastCommandBy { get; set; }
        public DateTime LastCommandDate { get; set; }

        public Schedule()
        {
            DefaultDuration = 3;
            DefaultWarmupTime = 40;
        	DefaultBreakDuration = 15;
        	DefaultAwardsDuration = 30;
            NumberOfPanels = 2;
            Days = new List<ScheduleDay>();
			DivisionPanels = new Dictionary<string, string>();
        }

        public void Update(ScheduleCreateCommand command)
        {
            CompetitionId = command.CompetitionId;
            Days = command
                        .Days
                        .Select(x => new ScheduleDay(x.AddHours(14))) //start at 8 am - HACK: add an additional 6 hours for UTC - total, ugly hack
                        .ToList();   

            this.RegisterCommand(command);
        }

        public void Update(SchedulingEditCommand command)
        {
            DefaultDuration = command.DefaultDuration;
            DefaultWarmupTime = command.DefaultWarmupTime;
        	DefaultBreakDuration = command.DefaultBreakDuration;
        	DefaultAwardsDuration = command.DefaultAwardsDuration;
            NumberOfPanels = command.NumberOfPanels;
            Days = command.Days;
        	DivisionPanels = command.DivisionPanels;

            this.RegisterCommand(command);
        }

        public static string FormatId(string competitionId)
        {
            return competitionId + "/schedule";
        }

        public string GenerateId()
        {
            return FormatId(CompetitionId);
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
            public DateTime PerformanceTime { get; set; }
            public int WarmupTime { get; set; }
            public int Duration { get; set; }
            public int Index { get; set; }
            public string Template { get; set; }

            public PerformanceEntry(Dictionary<string, string> entry)
            {
                RegistrationId = entry["registrationId"];
                PerformanceTime = DateTime.Parse(entry["time"]);
                WarmupTime = int.Parse(entry["warmupTime"]);
                Duration = int.Parse(entry["duration"]);
                Index = int.Parse(entry["index"]);
                Template = entry["template"];
            }
        }
    }
}