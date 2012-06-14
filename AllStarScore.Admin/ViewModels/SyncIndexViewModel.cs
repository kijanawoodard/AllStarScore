namespace AllStarScore.Admin.ViewModels
{
    public class SyncIndexViewModel
    {
        public string CompetitionId { get; set; }
        public string Hash { get; set; }

        public SyncIndexViewModel(string competitionId)
        {
            CompetitionId = competitionId;
            Hash = "Hello";
        }
    }
}