using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Scoring.Infrastructure.Indexes
{
    public class JudgeScoreIndex : AbstractIndexCreationTask<JudgeScore, JudgeScoreIndex.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string CompetitionId { get; set; }
            public string PerformanceId { get; set; }
            public string JudgeId { get; set; }
            public Dictionary<string, ScoreEntry> Scores { get; set; }
            public decimal GrandTotal { get; set; }
            public decimal GrandTotalServer { get; set; }
            public bool GrandTotalChecks { get; set; }

            public Result()
            {
                Scores = new Dictionary<string, ScoreEntry>();
            }
        }

        public JudgeScoreIndex()
        {
            Map = scores => from score in scores
                            select new
                                   {
                                       score.Id,
                                       score.PerformanceId,
                                       score.JudgeId,
                                       //score.CompetitionId
                                   };
        
            TransformResults =
                (database, results) => from result in results
                                       select new
                                              {
                                                  result.Id,
                                                  result.CompetitionId,
                                                  result.PerformanceId,
                                                  result.JudgeId,
                                                  result.Scores,
                                                  result.GrandTotal,
                                                  result.GrandTotalServer,
                                                  result.GrandTotalChecks
                                              };
        }
    }
}