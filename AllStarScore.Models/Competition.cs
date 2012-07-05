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

        public int NumberOfDays { get; set; }
        public int NumberOfPanels { get; set; }

        public bool IsWorldsCompetition { get; set; }

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




        public bool Equals(Competition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Competition)) return false;
            return Equals((Competition) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Id: {1}, FirstDay: {2}", Name, Id, FirstDay);
        }
    }
}