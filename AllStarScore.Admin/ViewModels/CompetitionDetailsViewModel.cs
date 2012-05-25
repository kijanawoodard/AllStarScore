using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class CompetitionDetailsViewModel
    {
        public Competition Competition { get; set; }

        public CompetitionDetailsViewModel(Competition competition)
        {
            Competition = competition;
        }
    }

    public class RegistrationIndexRequestModel
    {
        public int CompetitionId { get; set; }
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
        public int CompetitionId { get; set; }
        public int GymId { get; set; }

        public RegistrationTeamsViewModel()
        {
            
        }

        public RegistrationTeamsViewModel(int competitionid, int gymid)
        {
            CompetitionId = competitionid;
            GymId = gymid;
        }
    }
}