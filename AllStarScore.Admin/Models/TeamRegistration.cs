using System.Collections.Generic;
using System.Collections.ObjectModel;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.ViewModels;
using AllStarScore.Extensions;

namespace AllStarScore.Admin.Models
{
    public class TeamRegistration
    {
        public string Id { get; set; }
        
        public string CompetitionId { get; set; }
        public string GymId { get; set; }

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

            TeamName = command.TeamName.TrimSafely();
            ParticipantCount = command.ParticipantCount;
            DivisionId = command.DivisionId;
            IsShowTeam = command.IsShowTeam;

            History.Add(command);
        }

        public void Update(RegistrationEditCommand command)
        {
            TeamName = command.TeamName.TrimSafely();
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
}