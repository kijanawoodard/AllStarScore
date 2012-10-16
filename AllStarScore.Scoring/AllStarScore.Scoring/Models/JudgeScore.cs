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

    public class ScoringMap
    {
		public Dictionary<string, string> ScoreSheets
		{
			get
			{
				return new Dictionary<string, string>()
                       {
                           {"scoring-level1", "levels-level1-template"},
                           {"scoring-level2", "levels-level2-template"},
                           {"scoring-level3", "levels-level3-template"},
                           {"scoring-level4", "levels-level4-template"},
                           {"scoring-division42", "division-42-template"},
                           {"scoring-level5", "levels-level5-template"},
                           {"scoring-recreation", "levels-level5-template"},
                           {"scoring-worlds", "levels-level5-template"},
                           {"scoring-level6", "levels-level6-template"},
                           {"scoring-school", "levels-school-template"},
                           {"scoring-dance", "levels-dance-template"},
						   {"scoring-jazz", "division-jazz-template"},
                           {"judges-deductions", "judges-deductions-template"},
                           {"judges-legalities", "judges-legalities-template"}
                       };
			}
		}

        public Dictionary<string, string> Templates
        {
            get
            {
                return new Dictionary<string, string>()
                       {
                           {"scoring-level1", "all_star_template"},
                           {"scoring-level2", "all_star_template"},
                           {"scoring-level3", "all_star_template"},
                           {"scoring-level4", "lall_star_template"},
                           {"scoring-division42", "all_star_template"},
                           {"scoring-level5", "all_star_template"},
                           {"scoring-level6", "all_star_template"},
                           {"scoring-worlds", "all_star_template"},
                           {"scoring-recreation", "all_star_template"},
                           {"scoring-school", "single_column_template"},
                           {"scoring-dance", "single_column_template"},
                           {"scoring-jazz", "single_column_template"},
                           {"judges-deductions", "single_value_template"},
                           {"judges-legalities", "single_value_template"}
                       };
            }
        }

        public Dictionary<string, IScoringDefinition> Categories
        {
            get
            {
                return new Dictionary<string, IScoringDefinition>()
                       {
                           {"scoring-level1", new Level1ScoringDefinition()},
                           {"scoring-level2", new Level2ScoringDefinition()},
                           {"scoring-level3", new Level3ScoringDefinition()},
                           {"scoring-level4", new Level4ScoringDefinition()},
                           {"scoring-division42", new Level42ScoringDefinition()},
                           {"scoring-level5", new Level5ScoringDefinition()},
                           {"scoring-recreation", new Level5ScoringDefinition()},
                           {"scoring-worlds", new Level5ScoringDefinition()},
                           {"scoring-level6", new Level6ScoringDefinition()},
                           {"scoring-school", new SchoolScoringDefinition()},
//                           {"scoring-jazz", "division-jazz-template"},
                           {"judges-deductions", new DeductionsScoringDefinition()},
                           {"judges-legalities", new LegalitiesScoringDefinition()}
                       };
            }
        }
    }

    //why decimal? was getting some friction from Raven, plus http://stackoverflow.com/a/1165788/214073
    public class ScoreEntry
    {
        public decimal Base { get; set; }
        public decimal Execution { get; set; }

        public decimal Total { get { return (Base + Execution).RoundUp(1); } }
    }

    public class ScoringCategory
    {
        public string Display { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public bool IncludeExectionScore { get; set; }
    }

    public interface IScoringDefinition { }

    public abstract class AllStarScoringDefinition : IScoringDefinition
    {
        public ScoringCategory Stunts { get; set; }
        public ScoringCategory Pyramids { get; set; }
        public ScoringCategory Tosses { get; set; }
        public ScoringCategory StandardTumbling { get; set; }
        public ScoringCategory RunningTumbling { get; set; }
        public ScoringCategory Jumps { get; set; }
        public ScoringCategory MotionsDance { get; set; }
        public ScoringCategory FormationsTransitions { get; set; }
        public ScoringCategory PerformanceCategory { get; set; }
        public ScoringCategory SkillsCreativity { get; set; }
        public ScoringCategory RoutineCreativity { get; set; }

        protected AllStarScoringDefinition(int min, int max)
        {
            Stunts = new ScoringCategory { Display = "Stunts", Min = min, Max = max, IncludeExectionScore = true };
            Pyramids = new ScoringCategory { Display = "Pyramids", Min = min, Max = max, IncludeExectionScore = true };
            Tosses = new ScoringCategory { Display = "Tosses", Min = min, Max = max, IncludeExectionScore = true };
            StandardTumbling = new ScoringCategory { Display = "Standard Tumbling", Min = min, Max = max, IncludeExectionScore = true };
            RunningTumbling = new ScoringCategory { Display = "Running Tumbling", Min = min, Max = max, IncludeExectionScore = true };
            Jumps = new ScoringCategory { Display = "Jumps", Min = min, Max = max, IncludeExectionScore = true };
            MotionsDance = new ScoringCategory { Display = "Motions / Dance", Min = min, Max = max, IncludeExectionScore = true };
            FormationsTransitions = new ScoringCategory { Display = "Formations / Transitions", Min = min, Max = max, IncludeExectionScore = true };
            PerformanceCategory = new ScoringCategory { Display = "Performance", Min = min, Max = max, IncludeExectionScore = false };
            SkillsCreativity = new ScoringCategory { Display = "Skills Creativity", Min = 0, Max = 5, IncludeExectionScore = false };
            RoutineCreativity = new ScoringCategory { Display = "Routine Creativity", Min = 0, Max = 5, IncludeExectionScore = false };
        }
    }

    public class Level1ScoringDefinition : AllStarScoringDefinition
    {
        public Level1ScoringDefinition() : base(3, 5) { }
    }

    public class Level2ScoringDefinition : AllStarScoringDefinition
    {
        public Level2ScoringDefinition() : base(4, 6) { }
    }

    public class Level3ScoringDefinition : AllStarScoringDefinition
    {
        public Level3ScoringDefinition() : base(5, 7) { }
    }

	public class Level4ScoringDefinition : AllStarScoringDefinition
	{
		public Level4ScoringDefinition() : base(6, 8) { }
	}

	public class Level42ScoringDefinition : AllStarScoringDefinition
	{
		public Level42ScoringDefinition() : base(6, 8)
		{
			StandardTumbling.Max = 6;
			RunningTumbling.Max = 6;
		}
	}

	public class Level5ScoringDefinition : AllStarScoringDefinition
	{
		public Level5ScoringDefinition() : base(8, 10) { }
	}

	public class Level6ScoringDefinition : AllStarScoringDefinition
	{
		public Level6ScoringDefinition() : base(8, 10)
		{
			Stunts.Max = 11;
			Pyramids.Max = 11;
			Tosses.Max = 11;
		}
	}

    public class DeductionsScoringDefinition : IScoringDefinition
    {
        public ScoringCategory Deductions { get; set; }

        public DeductionsScoringDefinition()
        {
            Deductions = new ScoringCategory() { Display = "Deductions", Min = 0, Max = 20, IncludeExectionScore = false};
        }
    }

    public class LegalitiesScoringDefinition : IScoringDefinition
    {
        public ScoringCategory Legalities { get; set; }

        public LegalitiesScoringDefinition()
        {
            Legalities = new ScoringCategory() { Display = "Legalities", Min = 0, Max = 20, IncludeExectionScore = false };
        }
    }

	public class SchoolScoringDefinition : IScoringDefinition
	{
		public ScoringCategory Stunts { get; set; }
		public ScoringCategory PyramidsTosses { get; set; }
		public ScoringCategory Tumbling { get; set; }
		public ScoringCategory Jumps { get; set; }
		public ScoringCategory MotionsDance { get; set; }
		public ScoringCategory Timing { get; set; }
		public ScoringCategory Transitions { get; set; }
		public ScoringCategory PerformanceCategory { get; set; }
		public ScoringCategory SchoolRepresentation { get; set; }
		public ScoringCategory OverallImpression { get; set; }

		public SchoolScoringDefinition()
		{
			Stunts = new ScoringCategory { Display = "Stunts", Min = 0, Max = 10, IncludeExectionScore = false };
			PyramidsTosses = new ScoringCategory { Display = "Pyramids / Tosses", Min = 0, Max = 10, IncludeExectionScore = false };
			Tumbling = new ScoringCategory { Display = "Tumbling", Min = 0, Max = 10, IncludeExectionScore = false };
			Timing = new ScoringCategory { Display = "Timing", Min = 0, Max = 10, IncludeExectionScore = false };
			Jumps = new ScoringCategory { Display = "Jumps", Min = 0, Max = 10, IncludeExectionScore = false };
			MotionsDance = new ScoringCategory { Display = "Motions / Dance", Min = 0, Max = 10, IncludeExectionScore = false };
			Transitions = new ScoringCategory { Display = "Transitions", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceCategory = new ScoringCategory { Display = "Performance", Min = 0, Max = 10, IncludeExectionScore = false };
			SchoolRepresentation = new ScoringCategory { Display = "School Representation", Min = 0, Max = 10, IncludeExectionScore = false };
			OverallImpression = new ScoringCategory { Display = "Overall Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}
}