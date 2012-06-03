using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionListViewModel
    {
        public List<TeamRegistrationByCompetition.Results> Competitions { get; set; }
        public IEnumerable<TeamRegistrationByCompetition.Results> Upcoming { get; set; }
        public IEnumerable<TeamRegistrationByCompetition.Results> Past { get; set; }

        public CompetitionListViewModel(List<TeamRegistrationByCompetition.Results> competitions)
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