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
    [NUnit.Framework.DescriptionAttribute("Average Score Reporting")]
    public partial class AverageScoreReportingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AverageScoreReporting.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Average Score Reporting", "In order that teams know how they did against the average of their division\r\nAs a" +
                    " Tabulator\r\nI want to average the total of each scoring category by division", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Average a Divisions")]
        [NUnit.Framework.CategoryAttribute("mytag")]
        public virtual void AverageADivisions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Average a Divisions", new string[] {
                        "mytag"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Division Id"});
            table1.AddRow(new string[] {
                        "performances-1",
                        "divisions-1"});
            table1.AddRow(new string[] {
                        "performances-2",
                        "divisions-1"});
            table1.AddRow(new string[] {
                        "performances-3",
                        "divisions-2"});
            table1.AddRow(new string[] {
                        "performances-4",
                        "divisions-2"});
            table1.AddRow(new string[] {
                        "performances-5",
                        "divisions-2"});
            table1.AddRow(new string[] {
                        "performances-6",
                        "divisions-3"});
            table1.AddRow(new string[] {
                        "performances-7",
                        "divisions-3"});
#line 8
 testRunner.Given("a set of Performances to average", ((string)(null)), table1);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Performance Id",
                        "Stunts",
                        "Pyramids",
                        "Tosses",
                        "Deductions",
                        "Legalities"});
            table2.AddRow(new string[] {
                        "performances-1",
                        "10",
                        "10",
                        "10",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "performances-1",
                        "20",
                        "20",
                        "20",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "performances-2",
                        "10",
                        "5",
                        "30",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "performances-2",
                        "20",
                        "10",
                        "40",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "performances-1",
                        "",
                        "",
                        "",
                        "1",
                        ""});
            table2.AddRow(new string[] {
                        "performances-1",
                        "",
                        "",
                        "",
                        "",
                        "1"});
            table2.AddRow(new string[] {
                        "performances-2",
                        "",
                        "",
                        "",
                        "2",
                        ""});
            table2.AddRow(new string[] {
                        "performances-2",
                        "",
                        "",
                        "",
                        "",
                        ".5"});
#line 17
  testRunner.And("a set of scores", ((string)(null)), table2);
#line 27
 testRunner.When("I Average the Scores");
#line 28
 testRunner.Then("divisions-1 should have Stunts score equal 15");
#line 29
 testRunner.Then("divisions-1 should have Pyramids score equal 11.3");
#line 30
 testRunner.Then("divisions-1 should have Tosses score equal 25");
#line 31
 testRunner.Then("divisions-1 should have Deductions score equal 1.5");
#line 32
 testRunner.Then("divisions-1 should have Legalities score equal .8");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Average a few Divisions")]
        public virtual void AverageAFewDivisions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Average a few Divisions", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Division Id"});
            table3.AddRow(new string[] {
                        "performances-1",
                        "divisions-1"});
            table3.AddRow(new string[] {
                        "performances-2",
                        "divisions-1"});
            table3.AddRow(new string[] {
                        "performances-3",
                        "divisions-2"});
            table3.AddRow(new string[] {
                        "performances-4",
                        "divisions-2"});
            table3.AddRow(new string[] {
                        "performances-5",
                        "divisions-2"});
            table3.AddRow(new string[] {
                        "performances-6",
                        "divisions-3"});
            table3.AddRow(new string[] {
                        "performances-7",
                        "divisions-3"});
#line 35
 testRunner.Given("a set of Performances to average", ((string)(null)), table3);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Performance Id",
                        "Stunts",
                        "Pyramids",
                        "Tosses",
                        "Deductions",
                        "Legalities"});
            table4.AddRow(new string[] {
                        "performances-1",
                        "10",
                        "10",
                        "10",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-1",
                        "20",
                        "20",
                        "20",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-2",
                        "10",
                        "5",
                        "30",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-2",
                        "20",
                        "10",
                        "40",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-1",
                        "",
                        "",
                        "",
                        "1",
                        ""});
            table4.AddRow(new string[] {
                        "performances-1",
                        "",
                        "",
                        "",
                        "",
                        "1"});
            table4.AddRow(new string[] {
                        "performances-2",
                        "",
                        "",
                        "",
                        "2",
                        ""});
            table4.AddRow(new string[] {
                        "performances-2",
                        "",
                        "",
                        "",
                        "",
                        ".5"});
            table4.AddRow(new string[] {
                        "performances-3",
                        "10",
                        "10",
                        "10",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-3",
                        "20",
                        "20",
                        "20",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-4",
                        "10",
                        "5",
                        "30",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-4",
                        "20",
                        "10",
                        "40",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-3",
                        "",
                        "",
                        "",
                        "1",
                        ""});
            table4.AddRow(new string[] {
                        "performances-3",
                        "",
                        "",
                        "",
                        "",
                        "1"});
            table4.AddRow(new string[] {
                        "performances-4",
                        "",
                        "",
                        "",
                        "2",
                        ""});
            table4.AddRow(new string[] {
                        "performances-4",
                        "",
                        "",
                        "",
                        "",
                        ".5"});
            table4.AddRow(new string[] {
                        "performances-6",
                        "10",
                        "10",
                        "10",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-6",
                        "20",
                        "20",
                        "20",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-7",
                        "10",
                        "5",
                        "30",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-7",
                        "20",
                        "10",
                        "40",
                        "",
                        ""});
            table4.AddRow(new string[] {
                        "performances-6",
                        "",
                        "",
                        "",
                        "1",
                        ""});
            table4.AddRow(new string[] {
                        "performances-6",
                        "",
                        "",
                        "",
                        "",
                        "1"});
            table4.AddRow(new string[] {
                        "performances-7",
                        "",
                        "",
                        "",
                        "2",
                        ""});
            table4.AddRow(new string[] {
                        "performances-7",
                        "",
                        "",
                        "",
                        "",
                        ".5"});
#line 44
  testRunner.And("a set of scores", ((string)(null)), table4);
#line 70
 testRunner.When("I Average the Scores");
#line 71
 testRunner.Then("divisions-1 should have Stunts score equal 15");
#line 72
 testRunner.Then("divisions-1 should have Pyramids score equal 11.3");
#line 73
 testRunner.Then("divisions-1 should have Tosses score equal 25");
#line 74
 testRunner.Then("divisions-1 should have Deductions score equal 1.5");
#line 75
 testRunner.Then("divisions-1 should have Legalities score equal .8");
#line 76
 testRunner.Then("divisions-2 should have Stunts score equal 15");
#line 77
 testRunner.Then("divisions-2 should have Pyramids score equal 11.3");
#line 78
 testRunner.Then("divisions-2 should have Tosses score equal 25");
#line 79
 testRunner.Then("divisions-2 should have Deductions score equal 1.5");
#line 80
 testRunner.Then("divisions-2 should have Legalities score equal .8");
#line 81
 testRunner.Then("divisions-3 should have Stunts score equal 15");
#line 82
 testRunner.Then("divisions-3 should have Pyramids score equal 11.3");
#line 83
 testRunner.Then("divisions-3 should have Tosses score equal 25");
#line 84
 testRunner.Then("divisions-3 should have Deductions score equal 1.5");
#line 85
 testRunner.Then("divisions-3 should have Legalities score equal .8");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
