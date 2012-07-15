using System;

namespace AllStarScore.Models
{
    public class Level
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string ScoringDefinition { get; set; }

        public string CompanyId { get; set; }
        
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}", Id, Name);
        }

        public bool Equals(Level other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Level)) return false;
            return Equals((Level) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}