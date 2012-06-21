using System.Linq;
using AllStarScore.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Scoring.Infrastructure.Indexes
{
    public class PerformancesIndex : AbstractIndexCreationTask<Performance>
    {
        public PerformancesIndex()
        {
            Map = performances => from performance in performances
                                  select new
                                         {
                                             performance.Id,
                                             performance.CompetitionId,
                                             performance.PerformanceTime,
                                         };
        }
    }
}