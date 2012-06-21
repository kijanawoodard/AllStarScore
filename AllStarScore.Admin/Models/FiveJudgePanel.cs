using System.Collections.Generic;
using System.Globalization;

namespace AllStarScore.Admin.Models
{
    public class FiveJudgePanel : IJudgePanel
    {
        public string Id { get { return "panels-fivejudge"; } }

        public string Designator { get; private set; }

        private PanelJudge PanelJudge1 { get; set; }
        private PanelJudge PanelJudge2 { get; set; }
        private PanelJudge PanelJudge3 { get; set; }

        private DeductionsJudge DeductionsJudge { get; set; }
        private LegalitiesJudge LegalitiesJudge { get; set; }

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

    public class ScoringMap
    {
        public Dictionary<string, string> All
        {
            get
            {
                return new Dictionary<string, string>()
                       {
                           {"levels-level1", "levels-level1-template"},
                           {"levels-level2", "levels-level2-template"},
                           {"levels-level3", "levels-level3-template"},
                           {"levels-level4", "levels-level4-template"},
                           {"division-42", "division-42-template"},
                           {"levels-level5", "levels-level5-template"},
                           {"levels-recreation", "levels-level5-template"},
                           {"levels-worlds", "levels-level5-template"},
                           {"levels-level6", "levels-level6-template"},
                           {"levels-school", "levels-school-template"},
                           {"division-jazz", "division-jazz-template"},
                           {"judges-deductions", "judges-deductions-template"},
                           {"judges-legalities", "judges-legalities-template"}
                       };
            }
        }
    }
}