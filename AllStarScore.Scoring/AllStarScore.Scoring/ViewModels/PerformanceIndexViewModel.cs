using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllStarScore.Models;

namespace AllStarScore.Scoring.ViewModels
{
    public class PerformanceIndexViewModel
    {
        public IEnumerable<Performance> Performances { get; set; }

        public PerformanceIndexViewModel(IEnumerable<Performance> performances)
        {
            Performances = performances;
        }
    }
}