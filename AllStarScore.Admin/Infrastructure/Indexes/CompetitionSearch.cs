using System;
using System.Linq;
using AllStarScore.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class CompetitionSearch : AbstractIndexCreationTask<Competition, CompetitionSearch.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime FirstDay { get; set; }
            public DateTime LastDay { get; set; }


        }

        public CompetitionSearch()
        {
            Map = competitions => from competition in competitions
                                  select new
                                         {
                                             competition.Id,
                                             competition.Name,
                                             competition.Description,
                                             competition.FirstDay,
                                             competition.LastDay,
                                             competition.CompanyId,

                                             Keywords = new string[]
                                                        {
                                                            competition.Name,
                                                            competition.Description
                                                        }
                                         };

            Index("Keywords", FieldIndexing.Analyzed);

            TransformResults =
                (database, competitions) => from competition in competitions
                                            select new
                                            {
                                                competition.Id,
                                                competition.Name,
                                                competition.Description,
                                                competition.FirstDay,
                                                competition.LastDay
                                            };
        }
    }
}