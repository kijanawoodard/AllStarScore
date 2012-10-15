using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class ScoreSheetsIndexViewModel
    {
        public string CompetitionId { get; set; }
        public CompetitionInfo Info { get; set; }
        public IJudgePanel JudgePanel { get; set; }
        public Schedule Schedule { get; set; } //TODO: clientside, get from competition info
        public Dictionary<string, TeamRegistrationByCompetitionResults> Registrations { get; set; } //TODO: clientside, get from competition info

        public ScoreSheetsIndexViewModel(CompetitionInfo competitionInfo)
        {
            CompetitionId = competitionInfo.Id;
            Info = competitionInfo;

            Schedule = competitionInfo.Schedule;
            JudgePanel = new FiveJudgePanel(new List<JudgeScore>());
        }
    }
}