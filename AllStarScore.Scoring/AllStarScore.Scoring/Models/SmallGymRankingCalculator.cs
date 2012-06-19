using System;
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
        public IEnumerable<TeamScore> Rank(IEnumerable<TeamScore> scores)
        {
            var result = scores
                            .OrderByDescending(x => x.TotalScore)
                            .ThenBy(x => x.GymName)
                            .ToList();
            
            var rank = 0;
            for (int i = 0; i < result.Count; i++)
            {
                if (i > 0 && result[i-1].TotalScore == result[i].TotalScore)
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

    public class SmallGymRankingCalculator : IRankingCalculator
    {
        private NaturalRankingCalculator _calculator;

        public IEnumerable<TeamScore> Rank(IEnumerable<TeamScore> scores)
        {
            _calculator = new NaturalRankingCalculator();
            var result = _calculator.Rank(scores).ToList();

            SetFirstPlace(result, x => x.IsLargeGym);
            SetFirstPlace(result, x => x.IsSmallGym);

            result = result
                        .OrderBy(x => x.Rank)
                        .ThenByDescending(x => x.TotalScore)
                        .ThenBy(x => x.GymName)
                        .ToList();

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
            return performances.Select(x => new TeamScore
                                            {
                                                CompetitionId = x.CompetitionId,
                                                RegistrationId = x.RegistrationId,
                                                PerformanceScores = new List<decimal> {x.FinalScore},
                                                GymName = x.GymName,
                                                TeamName = x.TeamName,
                                                GymLocation = x.GymLocation,
                                                IsSmallGym = x.IsSmallGym,
                                                IsShowTeam = x.IsShowTeam,
                                                DidNotCompete = x.DidNotCompete,
                                                ScoringComplete = x.ScoringComplete,
                                            });
        }
    }
}