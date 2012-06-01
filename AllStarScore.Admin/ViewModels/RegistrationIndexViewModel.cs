using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class RegistrationIndexViewModel
    {
        public Competition Competition { get; set; }
        public string GymId { get; set; }

        public RegistrationIndexViewModel(Competition competition, string gymid)
        {
            Competition = competition;
            GymId = gymid;
        }
    }
}