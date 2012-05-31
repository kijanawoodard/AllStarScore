using System.Collections.Generic;
using System.Collections.ObjectModel;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.ViewModels;

namespace AllStarScore.Admin.Models
{
    public class TeamRegistration
    {
        public int CompetitionId { get; set; }
        public int GymId { get; set; }

        public string Id { get; set; }
        public string TeamName { get; set; }
        public int ParticipantCount { get; set; }
        public string DivisionId { get; set; }
        public bool IsShowTeam { get; set; }
        
        public ICollection<ICommand> History { get; private set; }

        public TeamRegistration()
        {
            History = new Collection<ICommand>();
        }

        public void Update(RegistrationCreateCommand command)
        {
            CompetitionId = command.CompetitionId;
            GymId = command.GymId;
            
            TeamName = command.TeamName;
            ParticipantCount = command.ParticipantCount;
            DivisionId = command.DivisionId;
            IsShowTeam = command.IsShowTeam;

            History.Add(command);
        }

        public override bool Equals(object obj)
        {
            var target = obj as TeamRegistration;
            if (target == null) return false;

            return Id.Equals(target.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return TeamName;
        }
    }

    public class Division
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LevelId { get; set; }

        public ICollection<ICommand> History { get; private set; }

        public Division()
        {
            History = new Collection<ICommand>();
        }

        public override bool Equals(object obj)
        {
            var target = obj as Division;
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

    public class Level
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<ICommand> History { get; private set; }

        public Level()
        {
            History = new Collection<ICommand>();
        }

        public override bool Equals(object obj)
        {
            var target = obj as Division;
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