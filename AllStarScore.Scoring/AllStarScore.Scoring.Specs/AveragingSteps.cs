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
    public class AveragingSteps
    {
        private List<Performance> _performances;
        private List<JudgeScore> _scores;
        private AverageScoreReporting _reporting;

        [Given(@"a set of Performances to average")]
        public void GivenASetOfPerformancesToAverage(Table table)
        {
            _performances = table.CreateSet<Performance>().ToList();
        }

        [Given(@"a set of scores")]
        public void GivenASetOfScores(Table table)
        {
            _scores = new List<JudgeScore>();

            foreach (var row in table.Rows)
            {
                var score = new JudgeScore();
                var keys = row.Keys.ToList();
                score.PerformanceId = row[keys[0]];
                for (int i = 1; i < keys.Count; i++)
                {
                    var str = row[keys[i]];
                    if (string.IsNullOrWhiteSpace(str))
                        continue;

                    var value = decimal.Parse(str);
                    score.Scores.Add(keys[i], new ScoreEntry() { Base = value }); //total hack to get the value in
                }

                _scores.Add(score);
            }
        }

        [When(@"I Average the Scores")]
        public void WhenIAverageTheScores()
        {
            _reporting = new AverageScoreReporting(_performances, _scores);
        }

        [Then(@"(.*) should have (.*) score equal ([\d\.]*)")]
        public void ThenDivisionShouldHaveCategoryScoreEqualValue(string divisionId, string category, decimal value)
        {
            Assert.AreEqual(value, _reporting.Averages[divisionId][category]);
        }
    }
}
