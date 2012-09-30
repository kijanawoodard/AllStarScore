using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class CompetitionLayoutIndexViewModel
    {
        public CompetitionInfo Info { get; set; }
		public List<Performance> Performances { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public CompetitionLayoutIndexViewModel(CompetitionInfo info, ScoringMap scoringMap)
        {
            Info = info;
			ScoringMap = scoringMap;

			Performances = 
				info
					.Registrations
					.SelectMany(x => x.GetPerformances(info.Competition))
					.ToList();
            
        }
    }
}