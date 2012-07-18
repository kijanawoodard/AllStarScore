using System;
using System.ComponentModel.DataAnnotations;

namespace AllStarScore.Models.Commands
{
    public class CompetitionCreateCommand : ICommand, ICompanyCommand
    {
        [Required]
        public string CompetitionName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FirstDay { get; set; }

        public int NumberOfDays { get; set; }
        public int NumberOfPerformances { get; set; }
        public int NumberOfPanels { get; set; }
        public bool IsWorldsCompetition { get; set; }
        
        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}