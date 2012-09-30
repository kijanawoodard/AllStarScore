namespace AllStarScore.Scoring.ViewModels
{
    public class PerformanceIndexViewModel
    {
        public string CompetitionId { get; set; }

        public PerformanceIndexViewModel(string competitionId)
        {
            CompetitionId = competitionId;
        }
    }
}