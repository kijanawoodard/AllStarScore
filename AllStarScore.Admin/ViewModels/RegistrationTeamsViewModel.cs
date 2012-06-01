using System.Collections.Generic;

namespace AllStarScore.Admin.ViewModels
{
    public class RegistrationTeamsViewModel
    {
        public string CompetitionId { get; set; }
        public string GymId { get; set; }

        public List<TeamRegistrationViewModel> Teams { get; set; }
        public List<DivisionViewModel> Divisions { get; set; } 

        public RegistrationTeamsViewModel()
        {
            
        }

        public RegistrationTeamsViewModel(string competitionid, string gymid)
        {
            CompetitionId = competitionid;
            GymId = gymid;
        }
    }
}