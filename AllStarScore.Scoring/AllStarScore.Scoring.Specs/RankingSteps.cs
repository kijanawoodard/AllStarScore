using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AllStarScore.Scoring.Specs
{
    [Binding]
    public class RankingSteps
    {
        private IRankingCalculator _calculator;
        private TeamScoreReporting _reporting;
        private List<TeamScore> _first { get { return _reporting.Divisions.First().Scores.ToList(); } } 
        private List<PerformanceScore> _performances;

        [Given(@"a Small Gym Ranking Calculator")]
        public void GivenASmallGymRankingCalculator()
        {
            _calculator = new SmallGymRankingCalculator();
        }

        [Given(@"a Natural Gym Ranking Calculator")]
        public void GivenANaturalGymRankingCalculator()
        {
            _calculator = new NaturalRankingCalculator();
        }


        [Given(@"a set of Performances:")]
        public void GivenIHaveASetOfPerformances(Table table)
        {
			var performances = table.CreateSet<PerformanceScore>().ToList();
            var generator = new TeamScoreGenerator();
            var scores = generator.From(performances, null);
            _reporting = new TeamScoreReporting(scores);
        }

        [Given(@"a set of Performances to be grouped:")]
        public void GivenIHaveASetOfPerformances2(Table table)
        {
            _performances = table.CreateSet<PerformanceScore>().ToList();
        }

        [When(@"Performances are Grouped")]
        public void WhenPerformancesAreGrouped()
        {
            var generator = new TeamScoreGenerator();
            var scores = generator.From(_performances, null);
            _reporting = new TeamScoreReporting(scores);
        }

        [When(@"the TeamScores are ranked")]
        public void WhenTheTeamScoresAreRanked()
        {
            _reporting.Rank();
        }

        [Then(@"(.*) should be (\d+)st")]
        public void ThenDivisionWinnerShouldBeNst(string gym, int index)
        {
            Assert.AreEqual(gym, _first[index - 1].GymName);
        }

        [Then(@"(.*) should be (\d+)nd")]
        public void ThenDivisionWinnerShouldBeNnd(string gym, int index)
        {
            Assert.AreEqual(gym, _first[index - 1].GymName);
        }

        [Then(@"(.*) should be (\d+)rd")]
        public void ThenDivisionWinnerShouldBeNrd(string gym, int index)
        {
            Assert.AreEqual(gym, _first[index - 1].GymName);
        }

        [Then(@"(.*) should be (\d+)th")]
        public void ThenDivisionWinnerShouldBeNth(string gym, int index)
        {
            Assert.AreEqual(gym, _first[index - 1].GymName);
        }

        [Then(@"(.*) should be ranked (\d+)")]
        public void ThenTigerCheerShouldBeRanked(string gym, int rank)
        {
            var score = _reporting.Divisions.SelectMany(x => x.Scores).First(x => x.GymName == gym);
            Assert.AreEqual(rank, score.Rank);
        }

        [Then(@"(.*) should be ranked (\d+) in division and (\d+) in level and (\d+) overall")]
        public void ThenTigerCheerShouldBeRanked(string gym, int divisionRank, int levelRank, int overallRank)
        {
            var divisionScore = _reporting.Divisions.SelectMany(x => x.Scores).Single(x => x.GymName == gym).Rank;
            var levelScore = _reporting.Levels.SelectMany(x => x.Scores).Single(x => x.GymName == gym).Rank;
            var overallScore = _reporting.Overall.Scores.Single(x => x.GymName == gym).Rank;
            Assert.AreEqual(divisionRank, divisionScore);
            Assert.AreEqual(levelRank, levelScore);
            Assert.AreEqual(overallRank, overallScore);
        }

        [Then(@"the count of (.*) will be (\d+)")]
        public void ThenTheCountOfKeyWillBeExpected(string key, int expected)
        {
            var count = _reporting.Divisions.First(x => x.Key == key).Scores.Count;
            Assert.AreEqual(expected, count);
        }
    }
}
