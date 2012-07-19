using System;
using System.ComponentModel.DataAnnotations;

namespace AllStarScore.Models.Commands
{
    public class GymCreateCommand : ICommand, ICompanyCommand
    {
        [Required]
        public string GymName { get; set; }

        [Required]
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }

        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }

        public GymCreateCommand()
        {
            IsSmallGym = true;
        }
    }
}