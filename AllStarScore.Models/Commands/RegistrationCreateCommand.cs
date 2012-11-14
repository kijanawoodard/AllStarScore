using System;

namespace AllStarScore.Models.Commands
{
    public class RegistrationCreateCommand : ICommand, ICompanyCommand
    {
        public string CompetitionId { get; set; }
        public string GymId { get; set; }

        public string TeamName { get; set; }
        public string DivisionId { get; set; }
        public int ParticipantCount { get; set; }
        public bool IsShowTeam { get; set; }
		public bool IsWorldsTeam { get; set; }

        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}