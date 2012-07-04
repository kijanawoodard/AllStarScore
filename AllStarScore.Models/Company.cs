namespace AllStarScore.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        //thinking
        public bool IsConcernedAboutSmallGyms { get; set; }
        public string Logo { get; set; }

        public Company()
        {
            IsConcernedAboutSmallGyms = true;
        }
    }
}