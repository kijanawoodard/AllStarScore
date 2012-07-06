using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllStarScore.Models;
using AllStarScore.Models.Commands;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AllStarScore.Scoring.Specs
{
    [Binding]
    public class CompetitionSteps
    {
        private Competition _competition;
        private CompetitionCreateCommand _command;

        [Given(@"A Competition")]
        public void GivenACompetition()
        {
            _competition = new Competition();
        }

        [Given(@"A Competition Create Command")]
        public void GivenACompetitionCreateCommand()
        {
            _command = new CompetitionCreateCommand {CommandByUser = "admin", CommandWhen = DateTime.UtcNow};
        }

        [Given(@"The First Day is (.*)")]
        public void GivenTheFirstDayIs(DateTime date)
        {
            _competition.FirstDay = date;
        }


        [When(@"I set the Number of Days to (\d+)")]
        public void WhenISetTheNumberOfDaysTo(int numberOfDays)
        {
            _competition.NumberOfDays = numberOfDays;
        }

        [Then(@"the Last Day should be (.*)")]
        public void ThenTheLastDayShouldBe(DateTime date)
        {
            Assert.AreEqual(date, _competition.LastDay);
        }

        [When(@"I set the Number of Panels to (\d+)")]
        public void WhenISetTheNumberOfPanelsTo(int numberOfPanels)
        {
            _competition.NumberOfPanels = numberOfPanels;
        }

        [Then(@"there should be (\d+) panels")]
        public void ThenThereShouldBeNPanels(int numberOfPanels)
        {
            Assert.AreEqual(numberOfPanels, _competition.Panels.Count);
        }

        [Then(@"the panels should be (.*)")]
        public void ThenThePanelsShouldBe(string panelList)
        {
            var panels = panelList.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
            Assert.AreEqual(panels.Count, _competition.Panels.Count);

            for (int i = 0; i < panels.Count; i++)
            {
                panels[i] = panels[i].Trim();
                Assert.AreEqual(panels[i], _competition.Panels[i]);
            }
        }

        [Then(@"the Number of Days should be (\d+)")]
        public void ThenTheNumberOfDaysShouldBe(int count)
        {
            Assert.AreEqual(count, _competition.NumberOfDays);
        }

        [Then(@"the Number of Panels should be (\d+)")]
        public void ThenTheNumberOfPanelsShouldBe(int count)
        {
            Assert.AreEqual(count, _competition.NumberOfPanels);
            Assert.AreEqual(count, _competition.Panels.Count);
        }

        [When(@"The Create Command is processed by Update")]
        public void WhenTheCreateCommandIsProcessedByUpdate()
        {
            _competition.Update(_command);
        }

        [Then(@"The ICanBeUpdatedByCommand Properties are Correct")]
        public void ThenTheICanBeUpdatedByCommandPropertiesAreCorrect()

        {
            Assert.AreEqual(_command.CommandByUser, _competition.LastCommandBy);
            Assert.AreEqual(_command.CommandWhen, _competition.LastCommandDate);
            Assert.AreEqual("CompetitionCreateCommand", _competition.LastCommand);
        }
    }
}
