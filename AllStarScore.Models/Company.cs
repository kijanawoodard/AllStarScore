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
            return "companies/"; //get an incremental id
        }
    }
}