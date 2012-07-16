using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public class Gym : ICanBeUpdatedByCommand, IBelongToCompany, IGenerateMyId
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }

        public string CompanyId { get; set; }
        public string LastCommand { get; set; }
        public string LastCommandBy { get; set; }
        public DateTime LastCommandDate { get; set; }

        public void Update(GymCreateCommand command)
        {
            Name = command.GymName;
            Location = command.Location;
            IsSmallGym = command.IsSmallGym;

            this.RegisterCommand(command);
        }

        public void Update(GymEditCommand command)
        {
            command.GymName = command.GymName.Trim(); //did it this way to send trimmed data back to client...a bit hacky, but....meh
            Name = command.GymName;
            Location = command.Location;
            IsSmallGym = command.IsSmallGym;

            this.RegisterCommand(command);
        }

        public string GenerateId()
        {
            return CompanyId + "/gym/";
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Location: {2}", Id, Name, Location);
        }

        public bool Equals(Gym other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Gym)) return false;
            return Equals((Gym) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}