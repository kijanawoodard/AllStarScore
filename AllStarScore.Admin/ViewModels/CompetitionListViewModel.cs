using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AllStarScore.Admin.Infrastructure.Indexes;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionListViewModel
    {
        public List<TeamRegistrationStatsByCompetition.Results> Competitions { get; set; }
        public IEnumerable<TeamRegistrationStatsByCompetition.Results> Upcoming { get; set; }
        public IEnumerable<TeamRegistrationStatsByCompetition.Results> Past { get; set; }

        public CompetitionListViewModel(List<TeamRegistrationStatsByCompetition.Results> competitions)
        {
            Competitions = competitions;
            Upcoming = competitions
                            .Where(c => c.CompetitionFirstDay >= DateTime.Today)
                            .OrderBy(c => c.CompetitionFirstDay);

            Past = competitions
                        .Except(Upcoming)
                        .OrderByDescending(c => c.CompetitionFirstDay);
        }
    }
}