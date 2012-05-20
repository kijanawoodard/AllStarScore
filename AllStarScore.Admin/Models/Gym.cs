namespace AllStarScore.Admin.Models
{
    public class Gym
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }

        public override bool Equals(object obj)
        {
            var target = obj as Gym;
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