using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SyncIndexViewModel
    {
        public Competition Competition { get; set; }
        public string Hash { get; set; }

        public SyncIndexViewModel(Competition competition)
        {
            Competition = competition;
            Hash = "Hello";
        }
    }
}