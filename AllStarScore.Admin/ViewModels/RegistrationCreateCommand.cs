using System;
using AllStarScore.Admin.Infrastructure.Commands;

namespace AllStarScore.Admin.ViewModels
{
    public class RegistrationCreateCommand : ICommand
    {
        public string CompetitionId { get; set; }
        public string GymId { get; set; }

        public string TeamName { get; set; }
        public string DivisionId { get; set; }
        public int ParticipantCount { get; set; }
        public bool IsShowTeam { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}