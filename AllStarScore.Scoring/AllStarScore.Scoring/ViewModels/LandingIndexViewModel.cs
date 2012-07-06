using System.Collections.Generic;
using AllStarScore.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class LandingIndexViewModel
    {
        public List<CompetitionInfo> Competitions { get; set; }

        public LandingIndexViewModel(List<CompetitionInfo> competitions)
        {
            Competitions = competitions;
        }
    }
}