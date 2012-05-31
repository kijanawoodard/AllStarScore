using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Indexes;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionDetailsViewModel
    {
        public Competition Competition { get; set; }
        public List<TeamRegistrationByGym.Results> Stats { get; set; }

        public CompetitionDetailsViewModel(Competition competition, List<TeamRegistrationByGym.Results> stats)
        {
            Competition = competition;
            Stats = stats;
        }
    }

    public class RegistrationIndexRequestModel
    {
        public string CompetitionId { get; set; }
        public int GymId { get; set; }
    }

    public class RegistrationIndexViewModel
    {
        public Competition Competition { get; set; }
        public int GymId { get; set; }

        public RegistrationIndexViewModel(Competition competition, int gymid)
        {
            Competition = competition;
            GymId = gymid;
        }
    }

    public class RegistrationRegisterViewModel
    {
        public Competition Competition { get; set; }

        public RegistrationRegisterViewModel(Competition competition)
        {
            Competition = competition;
        }
    }

    public class RegistrationTeamsViewModel
    {
        public string CompetitionId { get; set; }
        public int GymId { get; set; }

        public List<TeamRegistrationViewModel> Teams { get; set; }
        public List<DivisionViewModel> Divisions { get; set; } 

        public RegistrationTeamsViewModel()
        {
            
        }

        public RegistrationTeamsViewModel(string competitionid, int gymid)
        {
            CompetitionId = competitionid;
            GymId = gymid;
        }
    }
}