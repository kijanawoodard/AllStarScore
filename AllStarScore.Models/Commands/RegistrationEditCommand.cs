using System;
using System.ComponentModel.DataAnnotations;

namespace AllStarScore.Models.Commands
{
    public class RegistrationEditCommand : ICommand
    {
        [Required]
        public string Id { get; set; }

        public string TeamName { get; set; }

        [Required]
        public string DivisionId { get; set; }

        [Required]
        public int ParticipantCount { get; set; }
        public bool IsShowTeam { get; set; }
		public bool IsWorldsTeam { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}