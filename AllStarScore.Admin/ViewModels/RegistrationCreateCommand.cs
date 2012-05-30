using System;
using AllStarScore.Admin.Infrastructure.Commands;

namespace AllStarScore.Admin.ViewModels
{
    public class RegistrationCreateCommand : ICommand
    {
        public int CompetitionId { get; set; }
        public int GymId { get; set; }

        public string TeamName { get; set; }
        public int DivisionId { get; set; }
        public bool IsShowTeam { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}