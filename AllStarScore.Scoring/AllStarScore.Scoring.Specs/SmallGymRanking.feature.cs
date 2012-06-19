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
    [NUnit.Framework.DescriptionAttribute("Small Gym Ranking")]
    public partial class SmallGymRankingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SmallGymRanking.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Small Gym Ranking", "In order to give an equal chance to small gyms\r\nAs an event producer\r\nI want the " +
                    "scores ranked with two champions for each scoring group", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Rank Division with Large Gym Winner")]
        [NUnit.Framework.CategoryAttribute("mytag")]
        public virtual void RankDivisionWithLargeGymWinner()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with Large Gym Winner", new string[] {
                        "mytag"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("a Small Gym Ranking Calculator");
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
#line 9
  testRunner.And("a set of Performances:", ((string)(null)), table1);
#line 15
 testRunner.When("the TeamScores are ranked");
#line 16
 testRunner.Then("Division Winner should be 1st");
#line 17
  testRunner.And("Division Winner should be ranked 1");
#line 18
  testRunner.And("High Spirit should be ranked 1");
#line 19
  testRunner.And("A Large Gym should be ranked 2");
#line 20
  testRunner.And("Tiger Cheer should be ranked 3");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with a tie amongst non-winners")]
        public virtual void RankDivisionWithATieAmongstNon_Winners()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with a tie amongst non-winners", ((string[])(null)));
#line 22
this.ScenarioSetup(scenarioInfo);
#line 23
 testRunner.Given("a Small Gym Ranking Calculator");
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
                        "41.397",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table2.AddRow(new string[] {
                        "A Small Gym",
                        "true",
                        "41.397",
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
#line 24
  testRunner.And("a set of Performances:", ((string)(null)), table2);
#line 33
 testRunner.When("the TeamScores are ranked");
#line 34
 testRunner.Then("Division Winner should be 1st");
#line 35
  testRunner.And("Division Winner should be ranked 1");
#line 36
  testRunner.And("High Spirit should be ranked 1");
#line 37
  testRunner.And("A Large Gym should be ranked 2");
#line 38
  testRunner.And("Another Large Gym should be ranked 3");
#line 39
  testRunner.And("A Small Gym should be ranked 3");
#line 40
  testRunner.And("A Small Gym should be 4th");
#line 41
  testRunner.And("Another Large Gym should be 5th");
#line 42
  testRunner.And("Tiger Cheer should be ranked 4");
#line 43
  testRunner.And("A New Gym should be ranked 5");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with a tie amongst winners")]
        public virtual void RankDivisionWithATieAmongstWinners()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with a tie amongst winners", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 46
 testRunner.Given("a Small Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table3.AddRow(new string[] {
                        "Tiger Cheer",
                        "true",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table3.AddRow(new string[] {
                        "High Spirit",
                        "true",
                        "42.293",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table3.AddRow(new string[] {
                        "Division Winner",
                        "false",
                        "45.933",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table3.AddRow(new string[] {
                        "A Large Gym",
                        "false",
                        "43.397",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table3.AddRow(new string[] {
                        "Another Large Gym",
                        "false",
                        "45.933",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table3.AddRow(new string[] {
                        "A Small Gym",
                        "true",
                        "45.933",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table3.AddRow(new string[] {
                        "A New Gym",
                        "true",
                        "38.397",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
#line 47
  testRunner.And("a set of Performances:", ((string)(null)), table3);
#line 56
 testRunner.When("the TeamScores are ranked");
#line 57
 testRunner.Then("Division Winner should be 3rd");
#line 58
  testRunner.And("Division Winner should be ranked 1");
#line 59
  testRunner.And("A Large Gym should be ranked 2");
#line 60
  testRunner.And("High Spirit should be ranked 3");
#line 61
  testRunner.And("A Small Gym should be ranked 1");
#line 62
  testRunner.And("Another Large Gym should be ranked 1");
#line 63
  testRunner.And("A Small Gym should be 1st");
#line 64
  testRunner.And("Another Large Gym should be 2nd");
#line 65
  testRunner.And("Tiger Cheer should be ranked 4");
#line 66
  testRunner.And("A New Gym should be ranked 5");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with a tie small gym non-natural winners")]
        public virtual void RankDivisionWithATieSmallGymNon_NaturalWinners()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with a tie small gym non-natural winners", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line 69
 testRunner.Given("a Small Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table4.AddRow(new string[] {
                        "Tiger Cheer",
                        "true",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table4.AddRow(new string[] {
                        "High Spirit",
                        "true",
                        "39.293",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table4.AddRow(new string[] {
                        "Division Winner",
                        "false",
                        "45.933",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table4.AddRow(new string[] {
                        "A Large Gym",
                        "false",
                        "43.397",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table4.AddRow(new string[] {
                        "Another Large Gym",
                        "false",
                        "44.933",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table4.AddRow(new string[] {
                        "A Small Gym",
                        "true",
                        "40.200",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table4.AddRow(new string[] {
                        "A New Gym",
                        "true",
                        "38.397",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
#line 70
  testRunner.And("a set of Performances:", ((string)(null)), table4);
#line 79
 testRunner.When("the TeamScores are ranked");
#line 80
 testRunner.Then("Division Winner should be 1st");
#line 81
  testRunner.And("Division Winner should be ranked 1");
#line 82
  testRunner.And("Tiger Cheer should be ranked 1");
#line 83
  testRunner.And("A Small Gym should be ranked 1");
#line 84
  testRunner.And("A Small Gym should be 2nd");
#line 85
  testRunner.And("Tiger Cheer should be 3rd");
#line 86
  testRunner.And("Another Large Gym should be ranked 2");
#line 87
  testRunner.And("A Large Gym should be ranked 3");
#line 88
  testRunner.And("High Spirit should be ranked 4");
#line 89
  testRunner.And("A New Gym should be ranked 5");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with no Small Gyms")]
        public virtual void RankDivisionWithNoSmallGyms()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with no Small Gyms", ((string[])(null)));
#line 92
this.ScenarioSetup(scenarioInfo);
#line 93
 testRunner.Given("a Small Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table5.AddRow(new string[] {
                        "Tiger Cheer",
                        "true",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table5.AddRow(new string[] {
                        "High Spirit",
                        "true",
                        "39.293",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table5.AddRow(new string[] {
                        "Division Winner",
                        "true",
                        "45.933",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table5.AddRow(new string[] {
                        "A Large Gym",
                        "true",
                        "43.397",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table5.AddRow(new string[] {
                        "Another Large Gym",
                        "true",
                        "44.933",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table5.AddRow(new string[] {
                        "A Small Gym",
                        "true",
                        "40.200",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table5.AddRow(new string[] {
                        "A New Gym",
                        "true",
                        "38.397",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
#line 94
  testRunner.And("a set of Performances:", ((string)(null)), table5);
#line 103
 testRunner.When("the TeamScores are ranked");
#line 104
 testRunner.Then("Division Winner should be 1st");
#line 105
  testRunner.And("Division Winner should be ranked 1");
#line 106
  testRunner.And("Another Large Gym should be ranked 2");
#line 107
  testRunner.And("A Large Gym should be ranked 3");
#line 108
  testRunner.And("Tiger Cheer should be ranked 4");
#line 109
  testRunner.And("A Small Gym should be ranked 4");
#line 110
  testRunner.And("High Spirit should be ranked 5");
#line 111
  testRunner.And("A New Gym should be ranked 6");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with no Large Gyms")]
        public virtual void RankDivisionWithNoLargeGyms()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with no Large Gyms", ((string[])(null)));
#line 113
this.ScenarioSetup(scenarioInfo);
#line 114
 testRunner.Given("a Small Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table6.AddRow(new string[] {
                        "Tiger Cheer",
                        "false",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table6.AddRow(new string[] {
                        "High Spirit",
                        "false",
                        "39.293",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table6.AddRow(new string[] {
                        "Division Winner",
                        "false",
                        "45.933",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table6.AddRow(new string[] {
                        "A Large Gym",
                        "false",
                        "43.397",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table6.AddRow(new string[] {
                        "Another Large Gym",
                        "false",
                        "44.933",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table6.AddRow(new string[] {
                        "A Small Gym",
                        "false",
                        "40.200",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table6.AddRow(new string[] {
                        "A New Gym",
                        "false",
                        "38.397",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
#line 115
  testRunner.And("a set of Performances:", ((string)(null)), table6);
#line 124
 testRunner.When("the TeamScores are ranked");
#line 125
 testRunner.Then("Division Winner should be 1st");
#line 126
  testRunner.And("Division Winner should be ranked 1");
#line 127
  testRunner.And("Another Large Gym should be ranked 2");
#line 128
  testRunner.And("A Large Gym should be ranked 3");
#line 129
  testRunner.And("Tiger Cheer should be ranked 4");
#line 130
  testRunner.And("A Small Gym should be ranked 4");
#line 131
  testRunner.And("High Spirit should be ranked 5");
#line 132
  testRunner.And("A New Gym should be ranked 6");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with Large Gym Winner and a Small Gym winner far down the list")]
        public virtual void RankDivisionWithLargeGymWinnerAndASmallGymWinnerFarDownTheList()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with Large Gym Winner and a Small Gym winner far down the list", ((string[])(null)));
#line 134
this.ScenarioSetup(scenarioInfo);
#line 135
 testRunner.Given("a Small Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table7.AddRow(new string[] {
                        "Tiger Cheer",
                        "true",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "Bear Cheer",
                        "true",
                        "41.200",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "Zebra Cheer",
                        "true",
                        "39.200",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "Division Winner",
                        "false",
                        "45.933",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "A Large Gym",
                        "false",
                        "43.397",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "A Large Gym2",
                        "false",
                        "43.297",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "A Large Gym3",
                        "false",
                        "43.197",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "A Large Gym4",
                        "false",
                        "42.397",
                        "registrations-8",
                        "divisions-1",
                        "levels-1"});
            table7.AddRow(new string[] {
                        "High Spirit",
                        "true",
                        "42.293",
                        "registrations-9",
                        "divisions-1",
                        "levels-1"});
#line 136
  testRunner.And("a set of Performances:", ((string)(null)), table7);
#line 148
 testRunner.When("the TeamScores are ranked");
#line 149
 testRunner.Then("Division Winner should be 1st");
#line 150
  testRunner.And("Division Winner should be ranked 1");
#line 151
  testRunner.And("High Spirit should be ranked 1");
#line 152
  testRunner.And("A Large Gym should be ranked 2");
#line 153
  testRunner.And("Bear Cheer should be ranked 6");
#line 154
  testRunner.And("Tiger Cheer should be ranked 7");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Rank Division with Large Gym Winner and a Small Gym winner far down the list With" +
            " a lot of ties in between")]
        public virtual void RankDivisionWithLargeGymWinnerAndASmallGymWinnerFarDownTheListWithALotOfTiesInBetween()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rank Division with Large Gym Winner and a Small Gym winner far down the list With" +
                    " a lot of ties in between", ((string[])(null)));
#line 156
this.ScenarioSetup(scenarioInfo);
#line 157
 testRunner.Given("a Small Gym Ranking Calculator");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "GymName",
                        "IsSmallGym",
                        "Final Score",
                        "Registration Id",
                        "Division Id",
                        "Level Id"});
            table8.AddRow(new string[] {
                        "Tiger Cheer",
                        "true",
                        "40.200",
                        "registrations-1",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "Bear Cheer",
                        "true",
                        "41.200",
                        "registrations-2",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "Zebra Cheer",
                        "true",
                        "39.200",
                        "registrations-3",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "Division Winner",
                        "false",
                        "45.933",
                        "registrations-4",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "A Large Gym",
                        "false",
                        "43.397",
                        "registrations-5",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "A Large Gym2",
                        "false",
                        "43.297",
                        "registrations-6",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "A Large Gym3",
                        "false",
                        "43.297",
                        "registrations-7",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "A Large Gym4",
                        "false",
                        "43.297",
                        "registrations-8",
                        "divisions-1",
                        "levels-1"});
            table8.AddRow(new string[] {
                        "High Spirit",
                        "true",
                        "42.293",
                        "registrations-9",
                        "divisions-1",
                        "levels-1"});
#line 158
  testRunner.And("a set of Performances:", ((string)(null)), table8);
#line 170
 testRunner.When("the TeamScores are ranked");
#line 171
 testRunner.Then("Division Winner should be 1st");
#line 172
  testRunner.And("Division Winner should be ranked 1");
#line 173
  testRunner.And("High Spirit should be ranked 1");
#line 174
  testRunner.And("A Large Gym should be ranked 2");
#line 175
  testRunner.And("Bear Cheer should be ranked 4");
#line 176
  testRunner.And("Tiger Cheer should be ranked 5");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
