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
        public IEnumerable<DateTime> CompetitionDays { get; set; }

        public IEnumerable<Level> Levels { get; set; }
        public IEnumerable<Division> Divisions { get; set; }
        public IEnumerable<Registration> Registrations { get; set; }
        public IEnumerable<Gym> Gyms { get; set; }
        public IEnumerable<PerformaceVM> Performances { get; set; } 
            
        public SchedulingEditViewModel(Schedule schedule
                                     , Competition competition
                                     , IEnumerable<Level> levels
                                     , IEnumerable<Division> divisions
                                     , IEnumerable<Gym> gyms 
                                     , IEnumerable<Registration> registrations
                                     , IEnumerable<PerformaceVM> performances 
                                     )
        {
            Schedule = schedule;
            Competition = competition;
            CompetitionDays = competition.Days.ToList();

            Levels = levels;
            Divisions = divisions;
            Gyms = gyms;
            Registrations = registrations;
            Performances = performances;
        }
    }
}