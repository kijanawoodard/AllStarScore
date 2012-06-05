using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Indexes;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditViewModel
    {
        public List<TeamRegistrationByCompetition.Results> Registrations { get; set; }

        public SchedulingEditViewModel(List<TeamRegistrationByCompetition.Results> registrations)
        {
            Registrations = registrations;
        }
    }
}