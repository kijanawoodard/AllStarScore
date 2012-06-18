using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using AllStarScore.Scoring.Infrastructure.Indexes;
using AllStarScore.Scoring.ViewModels;
using AllStarScore.Extensions;

namespace AllStarScore.Scoring.Models
{
    public class JudgeScore : IJudgeScoreId
    {
        public string Id { get { return this.CalculateJudgeScoreId(); } }
        public string PerformanceId { get; set; }
        public string JudgeId { get; set; }

        public Dictionary<string, ScoreEntry> Scores { get; set; }
        public decimal GrandTotal { get; set; }

        public decimal GrandTotalServer
        {
            get
            {
                return Scores.Values.Aggregate(0.0m, (agg, score) => agg + score.Total);
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

//        public ICollection<ICommand> History { get; private set; }

        public JudgeScore()
        {
            Scores = new Dictionary<string, ScoreEntry>();
//            History = new Collection<ICommand>();
        }

        public JudgeScore(string performanceId, string judgeId) : this()
        {
            PerformanceId = performanceId;
            JudgeId = judgeId;
        }

        public void Update(ScoreEntryUpdateCommand command)
        {
            Scores = command.Scores;
            GrandTotal = command.GrandTotal;

//            History.Add(command);
        }
    }

    public interface ITeamScoreCalculator
    {
        List<JudgeScoreByPerformance.Result> Scores { get; set; }
        decimal AveragePanelScore { get; set; }
        decimal FinalScore { get; set; }
    }

    public class FiveJudgePanelPerformanceScoreCalculator : ITeamScoreCalculator
    {
        public List<JudgeScoreByPerformance.Result> Scores { get; set; }
        public decimal AveragePanelScore { get; set; }
        public decimal FinalScore { get; set; }

        public FiveJudgePanelPerformanceScoreCalculator(List<JudgeScoreByPerformance.Result> scores)
        {
            Scores = scores;

            AveragePanelScore =
                DecimalExtension.RoundUp(scores
                               .Where(x => new[] {"1", "2", "3"}.Contains(x.JudgeId))
                               .Average(x => x.GrandTotalServer), 3);

            FinalScore =
                AveragePanelScore - DecimalExtension.RoundUp(scores
                                                   .Where(x => new[] {"D", "L"}.Contains(x.JudgeId))
                                                   .Sum(x => x.GrandTotalServer), 3);
        }    
    }

    public class ScoringMap
    {
        public Dictionary<string, string> Templates
        {
            get
            {
                return new Dictionary<string, string>()
                       {
                           {"levels-level1", "all_star_template"},
                           {"levels-level2", "all_star_template"},
                           {"levels-level3", "all_star_template"},
                           {"levels-level4", "lall_star_template"},
                           {"division-42", "all_star_template"},
                           {"levels-level5", "all_star_template"},
                           {"levels-level6", "all_star_template"},
                           {"levels-worlds", "all_star_template"},
                           {"levels-recreation", "all_star_template"},
                           {"levels-school", "single_column_template"},
                           {"levels-dance", "single_column_template"},
                           {"division-jazz", "single_column_template"},
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
                           {"levels-level1", new Level1ScoringDefinition()},
                           {"levels-level2", new Level2ScoringDefinition()},
                           {"levels-level3", new Level3ScoringDefinition()},
//                           ,
//                           {"levels-level4", "lall-star-template"},
//                           {"division-42", "all-star-template"},
//                           {"levels-level5", "all-star-template"},
//                           {"levels-recreation", "all-star-template"},
//                           {"levels-worlds", "all-star-template"},
//                           {"levels-level6", "all-star-template"},
//                           {"levels-school", "levels-school-template"},
//                           {"division-jazz", "division-jazz-template"},
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

        public decimal Total { get { return DecimalExtension.RoundUp((Base + Execution), 1); } }
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
}