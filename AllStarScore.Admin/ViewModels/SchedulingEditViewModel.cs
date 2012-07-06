using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditViewModel
    {
        public Schedule Schedule { get; set; }
        public Competition Competition { get; set; }
        public Dictionary<string, TeamRegistrationByCompetitionResults> Registrations { get; set; }
        public List<DivisionViewModel> Divisions { get; set; }
        public IEnumerable<DateTime> CompetitionDays { get; set; }

        public SchedulingEditViewModel(Schedule schedule
                                     , Competition competition
                                     , IEnumerable<TeamRegistrationByCompetitionResults> registrations
                                     , IEnumerable<DivisionViewModel> divisions)
        {
            Schedule = schedule;
            Competition = competition;
            Registrations = registrations.ToDictionary(r => r.Id, r => r);
            Divisions = divisions.ToList();
            CompetitionDays = competition.Days.ToList();
        }
    }
}