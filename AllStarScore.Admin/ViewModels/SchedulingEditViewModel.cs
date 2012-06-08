using System;
using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditViewModel
    {
        public Schedule Schedule { get; set; }
        public List<TeamRegistrationByCompetition.Results> Registrations { get; set; }
        public List<DivisionViewModel> Divisions { get; set; }
        public IEnumerable<DateTime> CompetitionDays { get; set; } 
    }
}