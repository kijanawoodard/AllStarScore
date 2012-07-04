using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoreSheetsDetailsViewModel
    {
        public IJudgePanel JudgePanel { get; set; }
        public Dictionary<string, string> ScoringMap { get; set; }
        public Schedule Schedule { get; set; }
        public Dictionary<string, TeamRegistrationByCompetitionResults> Registrations { get; set; }

        public ScoreSheetsDetailsViewModel(CompetitionInfo competition)
        {
            Schedule = competition.Schedule;
            Registrations = competition
                                .Registrations
                                .OrderBy(x => x.CreatedAt).ToDictionary(r => r.Id, r => r);

            //ScoringMap = new ScoringMap().All;
            //JudgePanel = new FiveJudgePanel();
        }
    }
}