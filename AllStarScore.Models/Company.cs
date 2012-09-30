using System;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public class Company : ICanBeUpdatedByCommand, IGenerateMyId
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        //thinking
        public bool IsConcernedAboutSmallGyms { get; set; }
        public string Logo { get; set; }

        public string LastCommand { get; set; }
        public string LastCommandBy { get; set; }
        public DateTime LastCommandDate { get; set; }

        public Company()
        {
            Name = "Your Company Name";
            IsConcernedAboutSmallGyms = true;
        }

        public void Update(CompanyCreateCommand command)
        {	
            this.RegisterCommand(command);
        }

        public string GenerateId()
        {
            return "company/"; //get an incremental id
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}", Id, Name);
        }

        public bool Equals(Company other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Company)) return false;
            return Equals((Company) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}