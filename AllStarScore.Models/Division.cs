using System;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public class Division : ICanBeUpdatedByCommand, IBelongToCompany, IGenerateMyId
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LevelId { get; set; }
        public string ScoringDefinition { get; set; }

        public string CompanyId { get; set; }
        public string LastCommand { get; set; }
        public string LastCommandBy { get; set; }
        public DateTime LastCommandDate { get; set; }

        public void Update(DivisionCreateCommand command)
        {
            Name = command.Name;
            LevelId = command.LevelId;
            
            this.RegisterCommand(command);
        }

		public void Update(DivisionEditCommand command)
        {
            Name = command.Name;
            
            this.RegisterCommand(command);
        }
		
        public static string FormatId(string companyId)
        {
            return companyId + "/divisions";
        }

        public static string FormatId(string companyId, string levelId)
        {
            var result = string.Format("{0}{1}/division/", FormatId(companyId), levelId.Replace(companyId, string.Empty));
            return result;
        }

        public string GenerateId()
        {
            return FormatId(CompanyId, LevelId);
        }

        public bool Equals(Division other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Division)) return false;
            return Equals((Division) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}", Id, Name);
        }
    }
}