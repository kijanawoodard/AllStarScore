namespace AllStarScore.Admin.ViewModels
{
    public class DivisionViewModel
    {
        public string DivisionId { get; set; }
        public string Division { get; set; }
        public string Level { get; set; }
    }

    public class TeamRegistrationViewModel
    {
        public string CompetitionId { get; set; }
        public int GymId { get; set; }

        public string Id { get; set; }
        public string DivisionId { get; set; }
        public string TeamName { get; set; }
        public int ParticipantCount { get; set; }
        public bool IsShowTeam { get; set; }
    }
}