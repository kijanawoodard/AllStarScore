using System.Collections.Generic;

namespace AllStarScore.Scoring.Models
{
    public class JudgeScore
    {
        public string JudgeId { get; set; }
        public Dictionary<string, float> Scores { get; set; }

        public JudgeScore()
        {
            Scores = new Dictionary<string, float>();
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

    public class ScoringCategory
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public bool IncludeExectionScore { get; set; }

        //        public float Score { get; set; }
        //        public float ExecutionScore { get; set; }
        //
        //        public float TotalScore { get { return Score + ExecutionScore; } }
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
        public ScoringCategory Performance { get; set; }
        public ScoringCategory SkillsCreativity { get; set; }
        public ScoringCategory RoutineCreativity { get; set; }

        //        public abstract string Key { get; }

        public IEnumerable<ScoringCategory> All
        {
            get
            {
                yield return Stunts;
                yield return Pyramids;
                yield return Tosses;
                yield return StandardTumbling;
                yield return RunningTumbling;
                yield return Jumps;
                yield return MotionsDance;
                yield return FormationsTransitions;
                yield return Performance;
                yield return SkillsCreativity;
                yield return RoutineCreativity;
            }
        }

        protected AllStarScoringDefinition(int min, int max)
        {
            Stunts = new ScoringCategory { Name = "stunts", Display = "Stunts", Min = min, Max = max, IncludeExectionScore = true };
            Pyramids = new ScoringCategory { Name = "pyramids", Display = "Pyramids", Min = min, Max = max, IncludeExectionScore = true };
            Tosses = new ScoringCategory { Name = "tosses", Display = "Tosses", Min = min, Max = max, IncludeExectionScore = true };
            StandardTumbling = new ScoringCategory { Name = "standardTumbling", Display = "Standard Tumbling", Min = min, Max = max, IncludeExectionScore = true };
            RunningTumbling = new ScoringCategory { Name = "runningTumbling", Display = "Running Tumbling", Min = min, Max = max, IncludeExectionScore = true };
            Jumps = new ScoringCategory { Name = "jumps", Display = "Jumps", Min = min, Max = max, IncludeExectionScore = true };
            MotionsDance = new ScoringCategory { Name = "motionsDance", Display = "Motions / Dance", Min = min, Max = max, IncludeExectionScore = true };
            FormationsTransitions = new ScoringCategory { Name = "formationsTransitions", Display = "Formations / Transitions", Min = min, Max = max, IncludeExectionScore = true };
            Performance = new ScoringCategory { Name = "performance", Display = "Performance", Min = min, Max = max, IncludeExectionScore = false };
            SkillsCreativity = new ScoringCategory { Name = "skillsCreativity", Display = "Skills Creativity", Min = 0, Max = 5, IncludeExectionScore = false };
            RoutineCreativity = new ScoringCategory { Name = "routineCreativity", Display = "Routine Creativity", Min = 0, Max = 5, IncludeExectionScore = false };
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