using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditViewModel
    {
        public List<Schedule.ScheduleDay> Schedule { get; set; }
        public List<TeamRegistrationByCompetition.Results> Registrations { get; set; }

        public SchedulingEditViewModel(Schedule schedule, List<TeamRegistrationByCompetition.Results> registrations)
        {
            Schedule = schedule.ScheduleDays;
            Registrations = registrations;
        }
    }
}