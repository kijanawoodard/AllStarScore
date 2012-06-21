namespace AllStarScore.Models
{
    public class Level
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DefaultScoringDefinition { get; set; }


        public Level()
        {
        }

        public override bool Equals(object obj)
        {
            var target = obj as Division;
            if (target == null) return false;

            return Id.Equals(target.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}