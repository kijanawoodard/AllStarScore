using System;
using System.ComponentModel.DataAnnotations;

namespace AllStarScore.Models.Commands
{
    public class CompetitionCreateCommand : ICommand 
    {
        public CompetitionCreateCommand()
        {
            FirstDay = DateTime.Today;
            LastDay = FirstDay;
        }

        [Required]
        public string CompetitionName { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FirstDay { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime LastDay { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}