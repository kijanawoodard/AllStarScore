using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AllStarScore.Extensions;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public class Registration : ICanBeUpdatedByCommand, IBelongToCompany, IBelongToCompetition, IGenerateMyId
    {
        public string Id { get; set; }
        public string TeamName { get; set; }
        public int ParticipantCount { get; set; }
        public string DivisionId { get; set; }
        public bool IsShowTeam { get; set; }

        public DateTime CreatedAt { get; set; }

        public string GymId { get; set; }
        public string CompetitionId { get; set; }
        public string CompanyId { get; set; }
        public string LastCommand { get; set; }
        public string LastCommandBy { get; set; }
        public DateTime LastCommandDate { get; set; }

        public Registration()
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

            this.RegisterCommand(command);
        }

        public void Update(RegistrationEditCommand command)
        {
            TeamName = command.TeamName.TrimSafely();
            ParticipantCount = command.ParticipantCount;
            DivisionId = command.DivisionId;
            IsShowTeam = command.IsShowTeam;

            this.RegisterCommand(command);
        }

        public string GenerateId()
        {
            var result = string.Format("{0}{1}/registration/", CompetitionId, GymId.Replace(CompanyId, string.Empty));
            return result;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, TeamName: {1}, GymId: {2}, DivisionId: {3}", Id, TeamName, GymId, DivisionId);
        }

        public bool Equals(Registration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Registration)) return false;
            return Equals((Registration) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
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