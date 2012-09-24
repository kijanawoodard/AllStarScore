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
        public bool IsWorldsTeam { get; set; }

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

        public IEnumerable<PerformaceVM> GetPerformances(Competition competition)
        {
            var performance = GeneratePerformance("1");
            yield return performance;

            if (competition.NumberOfDays == 1)
                yield break;

            performance = GeneratePerformance("2");
            if (IsWorldsTeam)
            {
                performance.DivisionId = performance.DivisionId; //TODO: get Worlds Division Id onto competition
            }

            yield return performance;
        }

        private PerformaceVM GeneratePerformance(string id)
        {
            return new PerformaceVM
                   {
                       Id = string.Format("{0}/performances/{1}/performance/{2}", CompetitionId, Id.Substring(Id.IndexOf("gyms/", System.StringComparison.Ordinal)), id),
                       RegistrationId = Id,
                       DivisionId = DivisionId
                   };
        }

        public void Update(RegistrationCreateCommand command)
        {
            CompetitionId = command.CompetitionId;
            GymId = command.GymId;

            TeamName = command.TeamName.TrimSafely();
            ParticipantCount = command.ParticipantCount;
            DivisionId = command.DivisionId;
            IsShowTeam = command.IsShowTeam;
            IsWorldsTeam = command.IsWorldsTeam;

            this.RegisterCommand(command);
        }

        public void Update(RegistrationEditCommand command)
        {
            TeamName = command.TeamName.TrimSafely();
            ParticipantCount = command.ParticipantCount;
            DivisionId = command.DivisionId;
            IsShowTeam = command.IsShowTeam;
            IsWorldsTeam = command.IsWorldsTeam;

            this.RegisterCommand(command);
        }

        public static string FormatId(string competitionId)
        {
            return competitionId + "/registrations";
        }

        public static string FormatId(string competitionId, string gymId, string companyId)
        {
            var result = string.Format("{0}{1}/team/", FormatId(competitionId), gymId.Replace(companyId, string.Empty));
            return result;
        }

        public string GenerateId()
        {
            return FormatId(CompetitionId, GymId, CompanyId);
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, TeamName: {1}, DivisionId: {2}, IsShowTeam: {3}, IsWorldsTeam: {4}", Id, TeamName, DivisionId, IsShowTeam, IsWorldsTeam);
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