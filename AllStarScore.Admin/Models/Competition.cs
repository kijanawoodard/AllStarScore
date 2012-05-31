using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Models
{
    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime FirstDay { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastDay { get; set; }
    
        public ICollection<ICommand> History { get; private set; }

        public Competition()
        {
            History = new Collection<ICommand>();
        }

        public void Update(CompetitionCreateCommand command)
        {
            Name = command.Name;
            Description = command.Description;
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