using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using AllStarScore.Admin.Infrastructure.Commands;

namespace AllStarScore.Admin.Models
{
    public class Division
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LevelId { get; set; }

        public ICollection<ICommand> History { get; private set; }

        public Division()
        {
            History = new Collection<ICommand>();
        }

        public override bool Equals(object obj)
        {
            var target = obj as Division;
            if (target == null) return false;

            return Id.Equals(target.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return LevelId + "/" + Name;
        }
    }

    public interface IJudge
    {
        string Id { get; }
        string Designator { get; }
    }

    public class PanelJudge : IJudge
    {
        public string Id { get { return "judges-panel"; } }
        public string Designator { get; private set; }

        public PanelJudge(int designator)
        {
            Designator = designator.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class DeductionsJudge : IJudge
    {
        public string Id { get { return "judges-deductions"; } }
        public string Designator { get { return "D"; } }
    }

    public class LegalitiesJudge : IJudge
    {
        public string Id { get { return "judges-legalities"; } }
        public string Designator { get { return "L"; } }
    }

    public interface IJudgePanel
    {
        string Id { get; }
        string Designator { get; }
        IEnumerable<IJudge> Judges { get; }
    }

    public class JudgingPanels
    {
        
    }

    public class FiveJudgePanel : IJudgePanel
    {
        public string Id { get { return "panels-fivejudge"; } }

        public string Designator { get; private set; }

        public PanelJudge PanelJudge1 { get; set; }
        public PanelJudge PanelJudge2 { get; set; }
        public PanelJudge PanelJudge3 { get; set; }

        public DeductionsJudge DeductionsJudge { get; set; }
        public LegalitiesJudge LegalitiesJudge { get; set; }

        public IEnumerable<IJudge> Judges
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

        public FiveJudgePanel(string designator)
        {
            Designator = designator;

            PanelJudge1 = new PanelJudge(1);
            PanelJudge2 = new PanelJudge(2);
            PanelJudge3 = new PanelJudge(3);

            DeductionsJudge = new DeductionsJudge();
            LegalitiesJudge = new LegalitiesJudge();
        }
    }

    public class ScoringCategory
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public bool IncludeExectionScore { get; set; }

        public float Score { get; set; }
        public float ExecutionScore { get; set; }

        public float TotalScore { get { return Score + ExecutionScore; } }
    }

    public interface IScoringDefinition
    {
//        string Key { get; }
        IEnumerable<ScoringCategory> ScoringCategories { get; }
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

        public IEnumerable<ScoringCategory> ScoringCategories
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
            Stunts = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            Pyramids = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            Tosses = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            StandardTumbling = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            RunningTumbling = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            Jumps = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            MotionsDance = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            FormationsTransitions = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = true };
            Performance = new ScoringCategory { Min = min, Max = max, IncludeExectionScore = false };
            SkillsCreativity = new ScoringCategory { Min = 0, Max = 5, IncludeExectionScore = false };
            RoutineCreativity = new ScoringCategory { Min = 0, Max = 5, IncludeExectionScore = false };
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

    public class Foo
    {
        public string Template { get; set; }
        public IScoringDefinition ScoringDefinition { get; set; }
    }

    public class ScoringMap
    {
        public Dictionary<string, Foo> All 
        {
            get
            {
                return new Dictionary<string, Foo>()
                       {
                           {
                               "levels-level1",
                               new Foo { Template = "rough", ScoringDefinition = new Level1ScoringDefinition() }
                               }
                       };
            }
        }
    }
    public class JudgeScore
    {
        public IScoringDefinition Scoring { get; set; }
//        public string Panel { get; set; }
//        public string JudgeDesignator { get; set; }
//        public string JudgeOrdinal { get; set; }
        public string JudgeId { get; set; }
        public string PerformanceId { get; set; }
    }
}