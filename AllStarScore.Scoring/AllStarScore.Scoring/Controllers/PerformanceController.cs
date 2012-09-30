using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllStarScore.Models;
using AllStarScore.Scoring.ViewModels;

namespace AllStarScore.Scoring.Controllers
{
    public class PerformanceController : RavenController
    {
        public ActionResult Index(string id)
        {
            var model = new PerformanceIndexViewModel(id);
            return View(model);
        }
    }
}
