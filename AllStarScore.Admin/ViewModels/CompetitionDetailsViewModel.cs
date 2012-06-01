using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionDetailsViewModel
    {
        public Competition Competition { get; set; }
        public List<TeamRegistrationByGym.Results> Stats { get; set; }

        public CompetitionDetailsViewModel(Competition competition, List<TeamRegistrationByGym.Results> stats)
        {
            Competition = competition;
            Stats = stats;
        }
    }
}