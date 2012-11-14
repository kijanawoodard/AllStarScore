namespace AllStarScore.Admin.ViewModels
{
    public class TeamRegistrationViewModel //TODO: do we need this???
    {
        public string Id { get; set; }

        public string DivisionId { get; set; }
        public string TeamName { get; set; }
        public int ParticipantCount { get; set; }
        public bool IsShowTeam { get; set; }
		public bool IsWorldsTeam { get; set; }
    }
}