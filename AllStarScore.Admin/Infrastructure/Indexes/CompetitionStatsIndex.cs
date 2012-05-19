using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AllStarScore.Admin.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class CompetitionStatsIndex : AbstractIndexCreationTask<Competition, CompetitionStatsIndex.ReduceResult>
    {
        public class ReduceResult
        {
            public string Id { get; set; }
            public string Name { get; set; }

            [DataType(DataType.Date)]
            public DateTime FirstDay { get; set; }

            public int Count { get; set; }
        }

        public CompetitionStatsIndex()
        {
            Map = competitions => from competition in competitions
                                  select new 
                                    {
                                        competition.Id,
                                        competition.Name,
                                        competition.FirstDay,
                                        Count = 1
                                    };

            Reduce = results => from result in results
                                select new ReduceResult
                                {
                                    Id = result.Id,
                                    Name = result.Name,
                                    FirstDay = result.FirstDay,
                                    Count = result.Count 
                                };
//            Reduce = results => from result in results
//                                group result by new { result.Name }
//                                into g
//                                select new
//                                {
//                                    //Id = g.Key.Id,
//                                    g.Key.Name,
//                                    //g.Key.FirstDay, 
//                                    Count = g.Sum(x => x.Count)
//                                };
        }
    }
}