using System;
using System.Collections.Generic;
using AllStarScore.Extensions;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }

        public IEnumerable<DateTime> Days
        {
            get { return FirstDay.GetDateRange(LastDay); }
        }

        public string Display
        {
            get { return string.Format("{0} {1: MMM dd, yyyy}", Name, FirstDay); }
        }

        public void Update(CompetitionCreateCommand command)
        {
            Name = command.CompetitionName;
            Description = command.Description ?? string.Empty;
            FirstDay = command.FirstDay;
            LastDay = command.LastDay;
        }

        public override bool Equals(object obj)
        {
            var target = obj as Competition;
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
}