﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.8.1.0
//      SpecFlow Generator Version:1.8.0.0
//      Runtime Version:4.0.30319.269
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace AllStarScore.Scoring.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.8.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Natural Gym Ranking")]
    public partial class NaturalGymRankingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "NaturalGymRanking.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Natural Gym Ranking", "In give awards to teams\r\nAs an Event Producer\r\nI want to rank the teams by their " +
                    "scores", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with a tie amongst non-winners")]
        [NUnit.Framework.CategoryAttribute("mytag")]
        public virtual void RankDivisionWithATieAmongstNon_Winners()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with a tie amongst non-winners", new string[] {
                        "mytag"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("a Natural Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table1.AddRow(new string[] {
                        "Tiger Cheer",
                        "true",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table1.AddRow(new string[] {
                        "High Spirit",
                        "true",
                        "42.293",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table1.AddRow(new string[] {
                        "Division Winner",
                        "false",
                        "45.933",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table1.AddRow(new string[] {
                        "A Large Gym",
                        "false",
                        "43.397",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table1.AddRow(new string[] {
                        "Another Large Gym",
                        "false",
                        "41.397",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table1.AddRow(new string[] {
                        "A Small Gym",
                        "true",
                        "41.397",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table1.AddRow(new string[] {
                        "A New Gym",
                        "true",
                        "38.397",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
#line 9
  testRunner.And("a set of Performances:", ((string)(null)), table1);
#line 18
 testRunner.When("the TeamScores are ranked");
#line 19
 testRunner.Then("Division Winner should be 1st");
#line 20
  testRunner.And("Division Winner should be ranked 1");
#line 21
  testRunner.And("A Large Gym should be ranked 2");
#line 22
  testRunner.And("High Spirit should be ranked 3");
#line 23
  testRunner.And("A Small Gym should be ranked 4");
#line 24
  testRunner.And("Another Large Gym should be ranked 4");
#line 25
  testRunner.And("A Small Gym should be 4th");
#line 26
  testRunner.And("Another Large Gym should be 5th");
#line 27
  testRunner.And("Tiger Cheer should be ranked 5");
#line 28
  testRunner.And("A New Gym should be ranked 6");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with a tie amongst winners")]
        public virtual void RankDivisionWithATieAmongstWinners()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with a tie amongst winners", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
 testRunner.Given("a Natural Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table2.AddRow(new string[] {
                        "Tiger Cheer",
                        "true",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table2.AddRow(new string[] {
                        "High Spirit",
                        "true",
                        "42.293",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table2.AddRow(new string[] {
                        "Division Winner",
                        "false",
                        "45.933",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table2.AddRow(new string[] {
                        "A Large Gym",
                        "false",
                        "43.397",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table2.AddRow(new string[] {
                        "Another Large Gym",
                        "false",
                        "45.933",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table2.AddRow(new string[] {
                        "A Small Gym",
                        "true",
                        "45.933",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table2.AddRow(new string[] {
                        "A New Gym",
                        "true",
                        "38.397",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
#line 32
  testRunner.And("a set of Performances:", ((string)(null)), table2);
#line 41
 testRunner.When("the TeamScores are ranked");
#line 42
 testRunner.Then("Division Winner should be 3rd");
#line 43
  testRunner.And("Division Winner should be ranked 1");
#line 44
  testRunner.And("A Large Gym should be ranked 2");
#line 45
  testRunner.And("High Spirit should be ranked 3");
#line 46
  testRunner.And("A Small Gym should be ranked 1");
#line 47
  testRunner.And("Another Large Gym should be ranked 1");
#line 48
  testRunner.And("A Small Gym should be 1st");
#line 49
  testRunner.And("Another Large Gym should be 2nd");
#line 50
  testRunner.And("Tiger Cheer should be ranked 4");
#line 51
  testRunner.And("A New Gym should be ranked 5");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
