using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class CompetitionLayoutController : RavenController
    {
        [ChildActionOnly]
        public ActionResult Index(string id)
        {
            var info =
                RavenSession
                    .Load<CompetitionInfo>(id);

            var model = new CompetitionLayoutIndexViewModel(info, new ScoringMap());
            return PartialView(model);
        }
    }
}
