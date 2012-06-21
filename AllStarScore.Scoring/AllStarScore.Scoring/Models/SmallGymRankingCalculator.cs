using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Extensions;
using Newtonsoft.Json;

namespace AllStarScore.Scoring.Models
{
    public interface IRankingCalculator
    {
        IEnumerable<TeamScore> Rank(IEnumerable<TeamScore> scores);
    }

    public class NaturalRankingCalculator : IRankingCalculator
    {
        public virtual IEnumerable<TeamScore> Rank(IEnumerable<TeamScore> scores)
        {
            var rank = 1;
            var result = scores
                            .OrderByDescending(x => x.TotalScore)
                            .ThenBy(x => x.GymName)
                            .Select(x =>
                            {
                                x.Rank = rank++;
                                return x;
                            })
                            .ToList();

            result = DealWithTies(result);

            return result;
        }

        protected List<TeamScore> DealWithTies(List<TeamScore> scores)
        {
            var result = scores.ToList();

            var rank = 1;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Rank == 1)
                {
                    continue; //skip all the rank 1 temas
                }
                else if (result[i - 1].TotalScore == result[i].TotalScore)
                {
                    //everything's fine
                }
                else
                {
                    rank++;
                }

                result[i].Rank = rank;
            }
            return result;
        }
    }

    public class SmallGymRankingCalculator : NaturalRankingCalculator, IRankingCalculator
    {
        public override IEnumerable<TeamScore> Rank(IEnumerable<TeamScore> scores)
        {
            var result = base.Rank(scores).ToList();

            SetFirstPlace(result, x => x.IsLargeGym);
            SetFirstPlace(result, x => x.IsSmallGym);

            result = result
                        .OrderBy(x => x.Rank)
                        .ThenByDescending(x => x.TotalScore)
                        .ThenBy(x => x.GymName)
                        .ToList();
            
            result = DealWithTies(result);
            
            return result;
        }

        private void SetFirstPlace(IEnumerable<TeamScore> scores, Func<TeamScore, bool> which)
        {
            var list = scores.Where(which).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0 || list[i - 1].TotalScore == list[i].TotalScore)
                {
                    list[i].Rank = 1;
                }
                else break;
            }
        }
    }

    public class TeamScore
    {
        public int Rank { get; set; }

        public string CompetitionId { get; set; }
        public string RegistrationId { get; set; }
        public string LevelId { get; set; }
        public string DivisionId { get; set; }

        public List<decimal> PerformanceScores { get; set; }
        public decimal TotalScore { get { return PerformanceScores.Sum(); } }

        public string GymName { get; set; }
        public string TeamName { get; set; }
        public string GymLocation { get; set; }

        public string DivisionName { get; set; }
        public string LevelName { get; set; }

        public bool IsSmallGym { get; set; }
        public bool IsLargeGym { get { return !IsSmallGym; } }
        public bool IsShowTeam { get; set; }

        public bool DidNotCompete { get; set; }
        public bool ScoringComplete { get; set; }

        public TeamScore()
        {
            PerformanceScores = new List<decimal>();
        }
    }

    //creates TeamScore records from performances
    public class TeamScoreGenerator
    {
        public IEnumerable<TeamScore> From(IEnumerable<Performance> performances)
        {
            var scores =
                performances
                    .GroupBy(x => x.RegistrationId)
                    .Select(x => new TeamScore
                    {
                        CompetitionId = x.First().CompetitionId,
                        RegistrationId = x.First().RegistrationId,
                        LevelId = x.First().LevelId,
                        DivisionId = x.First().DivisionId,
                        PerformanceScores =
                            x.OrderBy(p => p.PerformanceTime)
                            .Select(p => p.FinalScore)
                            .ToList(),
                        GymName = x.First().GymName,
                        DivisionName = x.First().DivisionName,
                        LevelName = x.First().LevelName,
                        TeamName = x.First().TeamName,
                        GymLocation = x.First().GymLocation,
                        IsSmallGym = x.First().IsSmallGym,
                        IsShowTeam = x.First().IsShowTeam,
                        DidNotCompete = x.First().DidNotCompete,
                        ScoringComplete = x.First().ScoringComplete,
                    });

            return scores;
        }
    }

    public class TeamScoreReporting
    {
        public List<TeamScoreGroup> Divisions { get; set; }
        public List<TeamScoreGroup> Levels { get; set; }
        public TeamScoreGroup Overall { get; set; }

        [JsonIgnore]
        public List<TeamScoreGroup> All { get { return Divisions.Concat(Levels).Concat(new[] {Overall}).ToList(); } } 
        public TeamScoreReporting(IEnumerable<TeamScore> scores)
        {
            var list = scores.ToList();

            Divisions =
                list
                    .JsonCopy() //http://stackoverflow.com/a/222761/214073 //TODO: Blog about this
                    .GroupBy(x => x.DivisionId)
                    .Select(g => new TeamScoreGroup(g))
                    .ToList();

            Levels =
                list
                    .JsonCopy()
                    .GroupBy(x => x.LevelId)
                    .Select(g => new TeamScoreGroup(g))
                    .ToList();

            Overall = new TeamScoreGroup("overall", list);
        }

        public void Rank(IRankingCalculator calculator)
        {
            All.ForEach(x =>
            {
                x.Scores = calculator.Rank(x.Scores).ToList();
            });
        }
    }

    public class AverageScoreReporting
    {
        public Dictionary<string, Dictionary<string, decimal>> Averages { get; set; }
        
        public AverageScoreReporting(IEnumerable<Performance> performances, IEnumerable<JudgeScore> scores)
        {
            var all =
                from item in scores
                from score in item.Scores
                let divisionid = performances.First(p => p.Id == item.PerformanceId).DivisionId
                select new
                       {
                           DivisionId = divisionid,
                           Category = score.Key,
                           score.Value.Total 
                       };

            var averaged =
                from one in all
                group one by new {one.DivisionId, one.Category}
                into g
                select new
                       {
                           g.Key.DivisionId,
                           g.Key.Category,
                           Total = g.Average(x => x.Total).RoundUp(1)
                       };

            Averages =
                averaged
                    .GroupBy(x => x.DivisionId)
                    .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Category, y => y.Total));
        }
    }

    //http://stackoverflow.com/a/8508212/214073 - very loosely based
    public class TeamScoreGroup
    {
        public string Key { get; private set; }
        public List<TeamScore> Scores { get; set; }

        public TeamScoreGroup(IGrouping<string, TeamScore> grouping) : this(grouping.Key, grouping)
        {
        }

        public TeamScoreGroup(string key, IEnumerable<TeamScore> elements)
        {
            Key = key;
            Scores = elements.ToList();
        }
    }
}