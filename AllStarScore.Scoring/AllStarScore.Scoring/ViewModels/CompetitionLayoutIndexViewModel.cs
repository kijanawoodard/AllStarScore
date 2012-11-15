using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models;
using AllStarScore.Scoring.Controllers;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class CompetitionLayoutIndexViewModel
    {
        public CompetitionInfo Info { get; set; }
		public List<Performance> Performances { get; set; }
        public ScoringMap ScoringMap { get; set; }
		public IJudgePanel JudgePanel { get; set; }
		public SecurityContext SecurityContext { get; set; }

        public CompetitionLayoutIndexViewModel(CompetitionInfo info, ScoringMap scoringMap)
        {
            Info = info;
			ScoringMap = scoringMap;

			Performances = 
				info
					.Registrations
					.SelectMany(x => x.GetPerformances(info.Competition))
					.ToList();

        	var go =
        		Performances.Where(x => x.RegistrationId == "company/1/competitions/5/registrations/gyms/41/team/12").ToList();

			JudgePanel = new FiveJudgePanel(new List<JudgeScore>()); //TODO: If we need a different panel....
			SecurityContext = new SecurityContext();
        }
    }
}