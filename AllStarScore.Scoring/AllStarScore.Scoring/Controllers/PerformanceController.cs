﻿using System;
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
            var performances =
                RavenSession
                    .Query<Performance>()
                    .Where(x => x.CompetitionId == id)
                    .Take(int.MaxValue) //not expecting more than 100s, but likely slighly more than 128
                    .ToList();

            var model = new PerformanceIndexViewModel(performances);
            return View(model);
        }
    }
}