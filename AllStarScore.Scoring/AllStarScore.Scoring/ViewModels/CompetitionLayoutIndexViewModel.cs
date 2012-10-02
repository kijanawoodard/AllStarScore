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
		public ScoreSheetMap ScoreSheetMap { get; set; }
		public IJudgePanel JudgePanel { get; set; }

        public CompetitionLayoutIndexViewModel(CompetitionInfo info, ScoringMap scoringMap)
        {
            Info = info;
			ScoringMap = scoringMap;
			ScoreSheetMap = new ScoreSheetMap();

			Performances = 
				info
					.Registrations
					.SelectMany(x => x.GetPerformances(info.Competition))
					.ToList();

			JudgePanel = new FiveJudgePanel(new List<JudgeScore>()); //TODO: If we need a different panel....
        }
    }
}