using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using AllStarScore.Models;
using AllStarScore.Scoring.ViewModels;
using AllStarScore.Extensions;

namespace AllStarScore.Scoring.Models
{
	public class JudgeScore : IJudgeScoreId, IGenerateMyId
    {
		public string Id { get; set; }
        public string PerformanceId { get; set; }
        public string JudgeId { get; set; }

        public Dictionary<string, ScoreEntry> Scores { get; set; }
		public string Comments { get; set; }

        public decimal GrandTotal { get; set; }

        public decimal GrandTotalServer
        {
            get
            {
                return Scores.Values.Aggregate(0.0m, (agg, score) => agg + score.Total).RoundUp(3);
            }
        }

        public decimal GrandTotalDifference
        {
            get { return Math.Abs(GrandTotalServer - GrandTotal); }
        }
        public bool GrandTotalChecks
        {
            get
            {
                return GrandTotalDifference < .1m; //TODO: Testing
            }
        }

        public JudgeScore()
        {
            Scores = new Dictionary<string, ScoreEntry>();
        	Comments = string.Empty;
        }

        public JudgeScore(string performanceId, string judgeId) : this()
        {
            PerformanceId = performanceId;
            JudgeId = judgeId;
        }

        public void Update(ScoreEntryUpdateCommand command)
        {
            Scores = command.Scores;
        	Comments = command.Comments;
            GrandTotal = command.GrandTotal;
        }

		public static string FormatId(string performanceId)
		{
			return performanceId + "/scores/";
		}

		public static string FormatId(string performanceId, string judgeId)
		{
			return FormatId(performanceId) + judgeId;
		}

		public string GenerateId()
		{
			return FormatId(PerformanceId, JudgeId);
		}
    }

	public class PerformanceScore : IGenerateMyId
	{
		public string PerformanceId { get; set; }
		public string RegistrationId { get; set; }
		public string DivisionId { get; set; }

		public decimal TotalScore { get; set; }
		public Dictionary<string, Decimal> Scores { get; set; }
		public bool IsScoringComplete { get; set; }
		public bool DidNotCompete { get; set; }

		public bool IsFirstPerformance
		{
			get { return PerformanceId.EndsWith("/1"); }
		}

		public bool IsSecondPerformance
		{
			get { return PerformanceId.EndsWith("/2"); }
		}

		public PerformanceScore()
		{
			Scores = new Dictionary<string, decimal>();
		}

		public void Update(ITeamScoreCalculator calculator)
		{
			var performanceId = calculator.Scores.First().PerformanceId;
			PerformanceId = performanceId;
			TotalScore = calculator.FinalScore; 
			Scores = 
				calculator.Scores.SelectMany(x => x.Scores)
					.Select(x => new {Category = x.Key, Score = x.Value.Total})
					.GroupBy(x => x.Category)	
					.Select(g => new {Category = g.Key, AverageScore = g.Average(x => x.Score)})
					.ToDictionary(d => d.Category, d => d.AverageScore);
		}

		public void Update(MarkTeamScoringCompleteCommand command)
		{
			PerformanceId = command.PerformanceId;
			RegistrationId = command.RegistrationId;
			DivisionId = command.DivisionId;
			IsScoringComplete = true;
		}

		public void Update(MarkTeamScoringOpenCommand command)
		{
			IsScoringComplete = false;
		}

		public void Update(MarkTeamDidNotCompeteCommand command)
		{
			PerformanceId = command.PerformanceId;
			RegistrationId = command.RegistrationId;
			DivisionId = command.DivisionId;
			DidNotCompete = true;
		}

		public void Update(MarkTeamDidCompeteCommand command)
		{
			DidNotCompete = false;
		}

		public static string FormatId(string performanceId)
		{
			return performanceId.Replace("/registrations/", "/scores/");
		}

		public string GenerateId()
		{
			return FormatId(PerformanceId);
		}
	}
	
	public interface IJudge
    {
        string Id { get; }
        string Responsibility { get; }
    }

    public class PanelJudge : IJudge
    {
        public string Responsibility { get { return "judges-panel"; } }
        public string Id { get; private set; }

        public PanelJudge(int id)
        {
            Id = id.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class DeductionsJudge : IJudge
    {
        public string Responsibility { get { return "judges-deductions"; } }
        public string Id { get { return "D"; } }
    }

    public class LegalitiesJudge : IJudge
    {
        public string Responsibility { get { return "judges-legalities"; } }
        public string Id { get { return "L"; } }
    }

    public interface IJudgePanel
    {
        IEnumerable<IJudge> Judges { get; }
        IEnumerable<IJudge> PanelJudges { get; }
        ITeamScoreCalculator Calculator { get; }
    }

    public class FiveJudgePanel : IJudgePanel
    {
        public static readonly IJudge PanelJudge1 = new PanelJudge(1);
        public static readonly IJudge PanelJudge2 = new PanelJudge(2);
        public static readonly IJudge PanelJudge3 = new PanelJudge(3);
        public static readonly IJudge DeductionsJudge = new DeductionsJudge();
        public static readonly IJudge LegalitiesJudge = new LegalitiesJudge();

        private static IEnumerable<IJudge> AllJudges
        {
            get
            {
                yield return PanelJudge1;
                yield return PanelJudge2;
                yield return PanelJudge3;
                yield return DeductionsJudge;
                yield return LegalitiesJudge;
            }
        }

        public static readonly string[] JudgeIds = AllJudges.Select(x => x.Id).ToArray();

        public IEnumerable<IJudge> Judges { get { return AllJudges; } }
        public IEnumerable<IJudge> PanelJudges { get { return Judges.Take(3).ToList(); } }

        public ITeamScoreCalculator Calculator { get; set; }

		public FiveJudgePanel(List<JudgeScore> scores)
        {
            Calculator = new FiveJudgePanelPerformanceScoreCalculator(scores);
        }
    }

    public interface ITeamScoreCalculator
    {
        List<JudgeScore> Scores { get; set; }
        decimal AveragePanelScore { get; set; }
        decimal FinalScore { get; set; }
    }

    public class FiveJudgePanelPerformanceScoreCalculator : ITeamScoreCalculator
    {
        public List<JudgeScore> Scores { get; set; }
        public decimal AveragePanelScore { get; set; }
        public decimal FinalScore { get; set; }

        public FiveJudgePanelPerformanceScoreCalculator(List<JudgeScore> scores)
        {
            Scores = scores;

            if (!scores.Any())
                return;

            AveragePanelScore =
                scores
                    .Where(x => new[] {"1", "2", "3"}.Contains(x.JudgeId))
                    .Average(x => x.GrandTotalServer)
                    .RoundUp(3);

            FinalScore =
                AveragePanelScore - scores
                                        .Where(x => new[] {"D", "L"}.Contains(x.JudgeId))
                                        .Sum(x => x.GrandTotalServer)
                                        .RoundUp(3);
        }    
    }
}