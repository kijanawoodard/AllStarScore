using AllStarScore.Models;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class CompetitionLayoutIndexViewModel
    {
        public CompetitionImport Info { get; set; }
        public ScoringMap ScoringMap { get; set; }

        public CompetitionLayoutIndexViewModel(CompetitionImport info, ScoringMap scoringMap)
        {
            Info = info;
            ScoringMap = scoringMap;
        }
    }
}