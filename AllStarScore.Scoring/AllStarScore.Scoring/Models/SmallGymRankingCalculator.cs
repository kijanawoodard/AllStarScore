using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using AllStarScore.Models;

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
            var scores = performances
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
                                 TeamName = x.First().TeamName,
                                 GymLocation = x.First().GymLocation,
                                 IsSmallGym = x.First().IsSmallGym,
                                 IsShowTeam = x.First().IsShowTeam,
                                 DidNotCompete = x.First().DidNotCompete,
                                 ScoringComplete = x.First().ScoringComplete,
                             })
                             .ToList();

            var result = scores;
            return result;
        }
    }

    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {

        readonly List<TElement> _elements;

        public Grouping(IGrouping<TKey, TElement> grouping)
        {
            Key = grouping.Key;
            _elements = grouping.ToList();
        }

        public TKey Key { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    }
}