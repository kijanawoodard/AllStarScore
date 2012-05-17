using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionListViewModel
    {
        public CompetitionListViewModel(List<Competition> competitions)
        {
            Upcoming = competitions.Where(c => c.FirstDay >= DateTime.Today);
            Past = competitions.Except(Upcoming);
        }

        public IEnumerable<Competition> Upcoming { get; set; }
        public IEnumerable<Competition> Past { get; set; }
    }
}