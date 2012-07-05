using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoreSheetsDetailsViewModel
    {
        public string CompetitionId { get; set; }
        public CompetitionInfo Info { get; set; }
        public IJudgePanel JudgePanel { get; set; }
        public Dictionary<string, string> ScoringMap { get; set; }
        public Schedule Schedule { get; set; } //TODO: clientside, get from competition info
        public Dictionary<string, TeamRegistrationByCompetitionResults> Registrations { get; set; } //TODO: clientside, get from competition info

        public ScoreSheetsDetailsViewModel(CompetitionInfo competitionInfo)
        {
            CompetitionId = competitionInfo.Id;
            Info = competitionInfo;

            Schedule = competitionInfo.Schedule;
            Registrations = competitionInfo
                                .Registrations
                                .OrderBy(x => x.CreatedAt)
                                .ToDictionary(r => r.Id, r => r);

            ScoringMap = new ScoreSheetMap().All;
            JudgePanel = new FiveJudgePanel(new List<JudgeScoreIndex.Result>());
        }
    }
}