using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class ScoringController : RavenController
    {
        public ActionResult FiveJudgePanel(string id)
        {
            var performance =
                RavenSession
                    .Load<Performance>(id);

            var model = new ScoringFiveJudgePanelViewModel(performance, new ScoringMap());
            return View(model);
        }
    }

    public class ScoringMap
    {
        public Dictionary<string, string> All
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
                           {"division-jazz", "division-jazz-template"}
//                           ,
//                           {"judges-deductions", "judges-deductions-template"},
//                           {"judges-legalities", "judges-legalities-template"}
                       };
            }
        }
    }
}
