using System;

namespace AllStarScore.Models.Commands
{
    public class CompanyCreateCommand : ICommand
    {
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}