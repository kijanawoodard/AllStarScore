namespace AllStarScore.Models
{
    public class Division
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LevelId { get; set; }

        public Division()
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
            return LevelId + "/" + Name;
        }
    }
}