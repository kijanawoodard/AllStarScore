using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        string Designator { get; }
    }

    public class PanelJudge : IJudge
    {
        public string Designator { get; private set; }

        public PanelJudge(int designator) : this(designator.ToString(CultureInfo.InvariantCulture)){ }
        public PanelJudge(string designator)
        {
            Designator = designator;
        }
    }

    public class DeductionsJudge : IJudge
    {
        public string Designator { get { return "D"; } }
    }

    public class LegalitiesJudge : IJudge
    {
        public string Designator { get { return "L"; } }
    }

    public interface IJudgePanel
    {
        string Designator { get; }
        IEnumerable<IJudge> Judges { get; }
    }

    public class FiveJudgePanel : IJudgePanel
    {
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
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
    }

    public interface IScoringSet
    {
        IEnumerable<ScoringCategory> ScoringCategories { get; }
    }

    public class AllStarScoring : IScoringSet
    {
        public IEnumerable<ScoringCategory> ScoringCategories { get; set; }

        public AllStarScoring()
        {
            
        }
    }
}