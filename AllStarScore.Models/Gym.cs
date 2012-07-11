using System.Collections.Generic;
using System.Collections.ObjectModel;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public class Gym
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }
        
        public Gym()
        {
        }

        public void Update(GymCreateCommand command)
        {
            Name = command.GymName;
            Location = command.Location;
            IsSmallGym = command.IsSmallGym;
        }

        public void Update(GymEditCommand command)
        {
            command.GymName = command.GymName.Trim(); //did it this way to send trimmed data back to client...a bit hacky, but....meh
            Name = command.GymName;
            Location = command.Location;
            IsSmallGym = command.IsSmallGym;
        }

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