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

    public class RegistrationRegisterViewModel
    {
        public string CompetitionName { get; set; }

        public RegistrationRegisterViewModel(string competitionName)
        {
            CompetitionName = competitionName;
        }
    }
}