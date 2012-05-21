using System;
using System.ComponentModel.DataAnnotations;
using AllStarScore.Admin.Infrastructure.Commands;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionCreateCommand : ICommand 
    {
        public CompetitionCreateCommand()
        {
            FirstDay = DateTime.Today;
            LastDay = FirstDay;
        }

        [Required]
        public string Name { get; set; }

        [Required]
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