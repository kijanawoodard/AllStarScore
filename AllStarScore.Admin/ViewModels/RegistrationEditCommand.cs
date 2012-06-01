using System;
using System.ComponentModel.DataAnnotations;
using AllStarScore.Admin.Infrastructure.Commands;

namespace AllStarScore.Admin.ViewModels
{
    public class RegistrationEditCommand : ICommand
    {
        [Required]
        public string RegistrationId { get; set; }

        public string TeamName { get; set; }

        [Required]
        public string DivisionId { get; set; }

        [Required]
        public int ParticipantCount { get; set; }
        public bool IsShowTeam { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}