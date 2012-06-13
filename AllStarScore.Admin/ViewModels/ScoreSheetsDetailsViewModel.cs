using System.Collections.Generic;
using System.Linq;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class ScoreSheetsDetailsViewModel
    {
        public IJudgePanel JudgePanel { get; set; }
        public Dictionary<string, string> ScoringMap { get; set; }
        public Schedule Schedule { get; set; }
        public Dictionary<string, TeamRegistrationByCompetition.Results> Registrations { get; set; }

        public ScoreSheetsDetailsViewModel(Schedule schedule
                                           , IEnumerable<TeamRegistrationByCompetition.Results> registrations
                                           , Competition competition)
        {
            Schedule = schedule;
            Registrations = registrations.ToDictionary(r => r.Id, r => r);

            ScoringMap = new ScoringMap().All;
            JudgePanel = new FiveJudgePanel("A");
        }
    }
}