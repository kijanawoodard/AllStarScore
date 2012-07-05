using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllStarScore.Models;
using TechTalk.SpecFlow;

namespace AllStarScore.Scoring.Specs
{
    [Binding]
    public class CompetitionSteps
    {
        private Competition competition;

        [Given(@"A Competition")]
        public void GivenACompetition()
        {
            competition = new Competition();
        }


        [Given(@"The First Day is (.*)")]
        public void GivenTheFirstDayIs(DateTime date)
        {
            ScenarioContext.Current.Pending();
        }


        [When(@"I set the Number of Days to (\d+)")]
        public void WhenISetTheNumberOfDaysTo(int numberOfDays)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the Last Day should be (.*)")]
        public void ThenTheLastDayShouldBe752012(DateTime date)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
