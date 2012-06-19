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
        private List<TeamScore> _scores;
        private IRankingCalculator _calculator;


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
            _scores = generator.From(performances).ToList();
        }

        [When(@"the TeamScores are ranked")]
        public void WhenThePerformancesAreRanked()
        {
            _scores = _calculator.Rank(_scores).ToList();
        }

        [Then(@"(.*) should be (\d+)st")]
        public void ThenDivisionWinnerShouldBeNst(string gym, int index)
        {
            Assert.AreEqual(gym, _scores[index-1].GymName);
        }

        [Then(@"(.*) should be (\d+)nd")]
        public void ThenDivisionWinnerShouldBeNnd(string gym, int index)
        {
            Assert.AreEqual(gym, _scores[index-1].GymName);
        }

        [Then(@"(.*) should be (\d+)rd")]
        public void ThenDivisionWinnerShouldBeNrd(string gym, int index)
        {
            Assert.AreEqual(gym, _scores[index - 1].GymName);
        }

        [Then(@"(.*) should be (\d+)th")]
        public void ThenDivisionWinnerShouldBeNth(string gym, int index)
        {
            Assert.AreEqual(gym, _scores[index-1].GymName);
        }

        [Then(@"(.*) should be ranked (\d+)")]
        public void ThenTigerCheerShouldBeRanked(string gym, int rank)
        {
            var score = _scores.First(x => x.GymName == gym);
            Assert.AreEqual(rank, score.Rank);
        }
    }
}
