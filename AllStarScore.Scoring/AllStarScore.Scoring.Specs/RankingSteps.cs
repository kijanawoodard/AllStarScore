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
        private TeamScoreReporting _scores;
        private List<TeamScore> _first { get { return _scores.Divisions.First().ToList(); } } 
        private List<Performance> _performances;

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
            var performances = table.CreateSet<Performance>().ToList();
            var generator = new TeamScoreGenerator();
            _scores = generator.From(performances);
        }

        [Given(@"a set of Performances to be grouped:")]
        public void GivenIHaveASetOfPerformances2(Table table)
        {
            _performances = table.CreateSet<Performance>().ToList();
        }

        [When(@"Performances are Grouped")]
        public void WhenPerformancesAreGrouped()
        {
            var generator = new TeamScoreGenerator();
            _scores = generator.From(_performances);
        }

        [When(@"the TeamScores are ranked")]
        public void WhenTheTeamScoresAreRanked()
        {
            _scores.Rank(_calculator);
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
            var score = _scores.All.SelectMany(x => x.Elements).First(x => x.GymName == gym);
            Assert.AreEqual(rank, score.Rank);
        }

        [Then(@"(.*) should be ranked (\d+) in division and (\d+) in level and (\d+) overall")]
        public void ThenTigerCheerShouldBeRanked(string gym, int divisionRank, int levelRank, int overallRank)
        {
            var divisionScore = _scores.Divisions.SelectMany(x => x.Elements).Single(x => x.GymName == gym).Rank;
            var levelScore = _scores.Levels.SelectMany(x => x.Elements).Single(x => x.GymName == gym).Rank;
            var overallScore = _scores.Overall.Single(x => x.GymName == gym).Rank;
            Assert.AreEqual(divisionRank, divisionScore);
            Assert.AreEqual(levelRank, levelScore);
            Assert.AreEqual(overallRank, overallScore);
        }

        [Then(@"the count of (.*) will be (\d+)")]
        public void ThenTheCountOfKeyWillBeExpected(string key, int expected)
        {
            var count = _scores.All.First(x => x.Key == key).Count();
            Assert.AreEqual(expected, count);
        }
    }
}
