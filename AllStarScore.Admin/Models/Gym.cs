using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Models
{
    public class Gym
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }

        public ICollection<ICommand> History { get; private set; }
        
        public Gym()
        {
            History = new Collection<ICommand>();
        }

        public void Update(GymCreateCommand command)
        {
            Name = command.GymName;
            Location = command.Location;
            IsSmallGym = command.IsSmallGym;

            History.Add(command);
        }

        public void Update(GymEditCommand command)
        {
            command.GymName = command.GymName.Trim(); //did it this way to send trimmed data back to client...a bit hacky, but....meh
            Name = command.GymName;
            Location = command.Location;
            IsSmallGym = command.IsSmallGym;

            History.Add(command);
        }

        public override bool Equals(object obj)
        {
            var target = obj as Gym;
            if (target == null) return false;

            return Id.Equals(target.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Schedule
    {
        public List<ScheduleDay> ScheduleDays { get; set; }

        public Schedule(IEnumerable<DateTime> competitionDays)
        {
            ScheduleDays = competitionDays.Select(x => new ScheduleDay(x)).ToList();
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