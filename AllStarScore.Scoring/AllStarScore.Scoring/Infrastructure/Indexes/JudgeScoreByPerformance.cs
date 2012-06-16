using System.Collections.Generic;
using System.Linq;
using AllStarScore.Scoring.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Scoring.Infrastructure.Indexes
{
    public class JudgeScoreByPerformance : AbstractIndexCreationTask<JudgeScore, JudgeScoreByPerformance.Result>
    {
        public class Result
        {
            public string Id { get; set; }
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

        public JudgeScoreByPerformance()
        {
            Map = scores => from score in scores
                            select new
                                   {
                                       score.Id,
                                       score.PerformanceId,
                                       score.JudgeId
                                   };
        
            TransformResults =
                (database, results) => from result in results
                                       select new
                                              {
                                                  result.Id,
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