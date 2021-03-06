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
		public int CompetitionStyle { get; set; }
		public int NumberOfPerformances { get { return CompetitionStyle == 2 ? 2 : 1; } }
		public bool IsWorldsCompetition { get { return CompetitionStyle == 3; } }
        
        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}