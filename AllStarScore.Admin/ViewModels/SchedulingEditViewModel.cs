using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditViewModel
    {
        public Competition Competition { get; set; }
		public SchedulingEditViewModel(Competition competition)
        {
            Competition = competition;
        }
    }
}