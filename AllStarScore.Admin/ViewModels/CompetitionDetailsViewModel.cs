using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionDetailsViewModel
    {
        public Competition Competition { get; set; }
        public List<TeamRegistrationStatsByGym.Results> Stats { get; set; }

        public CompetitionDetailsViewModel(Competition competition, List<TeamRegistrationStatsByGym.Results> stats)
        {
            Competition = competition;
            Stats = stats;
        }
    }
}