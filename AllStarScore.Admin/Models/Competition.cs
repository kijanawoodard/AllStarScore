using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.Infrastructure.Utilities;
using AllStarScore.Admin.ViewModels;
using Newtonsoft.Json;

namespace AllStarScore.Admin.Models
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

        public ICollection<ICommand> History { get; private set; }

        public Competition()
        {
            History = new Collection<ICommand>();
        }

        public void Update(CompetitionCreateCommand command)
        {
            Name = command.CompetitionName;
            Description = command.Description ?? string.Empty;
            FirstDay = command.FirstDay;
            LastDay = command.LastDay;

            History.Add(command);
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