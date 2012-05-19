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
        public CompetitionListViewModel(List<CompetitionStatsIndex.ReduceResult> competitions)
        {
            Upcoming = competitions
                            .Where(c => c.FirstDay >= DateTime.Today)
                            .OrderBy(c => c.FirstDay);

            Past = competitions
                        .Except(Upcoming)
                        .OrderByDescending(c => c.FirstDay);
        }

        public IEnumerable<CompetitionStatsIndex.ReduceResult> Upcoming { get; set; }
        public IEnumerable<CompetitionStatsIndex.ReduceResult> Past { get; set; }
    }
}