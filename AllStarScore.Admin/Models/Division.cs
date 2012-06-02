using System.Collections.Generic;
using System.Collections.ObjectModel;
using AllStarScore.Admin.Infrastructure.Commands;

namespace AllStarScore.Admin.Models
{
    public class Division
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LevelId { get; set; }

        public ICollection<ICommand> History { get; private set; }

        public Division()
        {
            History = new Collection<ICommand>();
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