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

    public class FiveJudgePanelPerformanceScoreCalculator
    {
        public decimal AveragePanelScore { get; set; }
        public decimal FinalScore { get; set; }

        public FiveJudgePanelPerformanceScoreCalculator(List<JudgeScoreByPerformance.Result> scores)
        {
            AveragePanelScore =
                scores
                    .Where(x => new[] {"1", "2", "3"}.Contains(x.JudgeId))
                    .Average(x => x.GrandTotalServer)
                    .TruncateRound(3);

            FinalScore =
                AveragePanelScore - scores
                                        .Where(x => new[] {"D", "L"}.Contains(x.JudgeId))
                                        .Sum(x => x.GrandTotalServer)
                                        .TruncateRound(3);
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
                           {"levels-level1", "all-star-template"},
                           {"levels-level2", "all-star-template"},
                           {"levels-level3", "all-star-template"},
                           {"levels-level4", "lall-star-template"},
                           {"division-42", "all-star-template"},
                           {"levels-level5", "all-star-template"},
                           {"levels-recreation", "all-star-template"},
                           {"levels-worlds", "all-star-template"},
                           {"levels-level6", "all-star-template"},
                           {"levels-school", "levels-school-template"},
                           {"division-jazz", "division-jazz-template"},
                           {"judges-deductions", "judges-deductions-template"},
                           {"judges-legalities", "judges-legalities-template"}
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
                           {"levels-level3", new Level3ScoringDefinition()}
//                           ,
//                           {"levels-level4", "lall-star-template"},
//                           {"division-42", "all-star-template"},
//                           {"levels-level5", "all-star-template"},
//                           {"levels-recreation", "all-star-template"},
//                           {"levels-worlds", "all-star-template"},
//                           {"levels-level6", "all-star-template"},
//                           {"levels-school", "levels-school-template"},
//                           {"division-jazz", "division-jazz-template"},
//                           {"judges-deductions", "judges-deductions-template"},
//                           {"judges-legalities", "judges-legalities-template"}
                       };
            }
        }
    }

    //why decimal? was getting some friction from Raven, plus http://stackoverflow.com/a/1165788/214073
    public class ScoreEntry
    {
        public decimal Base { get; set; }
        public decimal Execution { get; set; }

        public decimal Total { get { return (Base + Execution).TruncateRound(1); } }
    }

    public class ScoringCategory
    {
        public string Display { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public bool IncludeExectionScore { get; set; }
    }

    public interface IScoringDefinition
    {
        //        string Key { get; }
//        IEnumerable<ScoringCategory> ScoringCategories { get; }
    }

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

        //        public abstract string Key { get; }

//        public IEnumerable<ScoringCategory> All
//        {
//            get
//            {
//                yield return Stunts;
//                yield return Pyramids;
//                yield return Tosses;
//                yield return StandardTumbling;
//                yield return RunningTumbling;
//                yield return Jumps;
//                yield return MotionsDance;
//                yield return FormationsTransitions;
//                yield return Performance;
//                yield return SkillsCreativity;
//                yield return RoutineCreativity;
//            }
//        }

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
        //        public override string Key { get { return "scoring-level1"; } }

        public Level1ScoringDefinition() : base(3, 5) { }
    }

    public class Level2ScoringDefinition : AllStarScoringDefinition
    {
        //        public override string Key { get { return "scoring-level2"; } }

        public Level2ScoringDefinition() : base(4, 6) { }
    }

    public class Level3ScoringDefinition : AllStarScoringDefinition
    {
        //        public override string Key { get { return "scoring-level2"; } }

        public Level3ScoringDefinition() : base(5, 7) { }
    }
}