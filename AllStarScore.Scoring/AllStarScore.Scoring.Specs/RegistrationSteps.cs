using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllStarScore.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AllStarScore.Scoring.Specs
{
    [Binding]
    public class RegistrationSteps
    {
        private Registration _registration;
        private string _registrationId;

        [Given(@"A new Registration")]
        public void GivenANewRegistration()
        {
            _registration = new Registration();
        }

        [Given(@"The company id is (.*)")]
        public void GivenTheCompanyIdIs(string id)
        {
            _registration.CompanyId = id;
        }

        [Given(@"The competition id is (.*)")]
        public void GivenTheCompetitionIdIs(string id)
        {
            _registration.CompetitionId = id;
        }

        [Given(@"The gym id is (.*)")]
        public void GivenTheGymIdIs(string id)
        {
            _registration.GymId = id;
        }

        [When(@"The Id is Generated")]
        public void WhenTheIdIsGenerated()
        {
            _registrationId = _registration.GenerateId();
        }

        [Then(@"The result should be (.*)")]
        public void ThenTheResultShouldBe(string id)
        {
            Assert.AreEqual(id, _registrationId);
        }
    }
}
