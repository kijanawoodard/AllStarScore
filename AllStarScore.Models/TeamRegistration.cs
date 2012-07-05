using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AllStarScore.Extensions;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
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

        public DateTime CreatedAt { get; set; }
        
        public TeamRegistration()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(RegistrationCreateCommand command)
        {
            CompetitionId = command.CompetitionId;
            GymId = command.GymId;

            TeamName = command.TeamName.TrimSafely();
            ParticipantCount = command.ParticipantCount;
            DivisionId = command.DivisionId;
            IsShowTeam = command.IsShowTeam;
        }

        public void Update(RegistrationEditCommand command)
        {
            TeamName = command.TeamName.TrimSafely();
            ParticipantCount = command.ParticipantCount;
            DivisionId = command.DivisionId;
            IsShowTeam = command.IsShowTeam;
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

    public class TeamRegistrationByCompetitionResults
    {
        public string Id { get; set; }
        public string CompetitionId { get; set; }
        public string GymId { get; set; }
        public string DivisionId { get; set; }
        public string LevelId { get; set; }

        public string TeamName { get; set; }
        public int ParticipantCount { get; set; }
        public bool IsShowTeam { get; set; }
        public string GymName { get; set; }
        public string GymLocation { get; set; }
        public bool IsSmallGym { get; set; }
        public string DivisionName { get; set; }
        public string LevelName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}