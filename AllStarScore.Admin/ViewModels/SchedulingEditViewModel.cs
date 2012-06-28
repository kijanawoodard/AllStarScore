using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditViewModel
    {
        public Schedule Schedule { get; set; }
        public Dictionary<string, TeamRegistrationByCompetition.Results> Registrations { get; set; }
        public List<DivisionViewModel> Divisions { get; set; }
        public IEnumerable<DateTime> CompetitionDays { get; set; }
        public int NumberOfPerformances { get; set; }

        public SchedulingEditViewModel(Schedule schedule
                                     , Competition competition
                                     , IEnumerable<TeamRegistrationByCompetition.Results> registrations
                                     , IEnumerable<DivisionViewModel> divisions)
        {
            Schedule = schedule;
            Registrations = registrations.ToDictionary(r => r.Id, r => r);
            Divisions = divisions.ToList();
            CompetitionDays = competition.Days.ToList();
            NumberOfPerformances = competition.NumberOfPerformances;
        }
    }
}