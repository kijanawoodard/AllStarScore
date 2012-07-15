using System;

namespace AllStarScore.Models.Commands
{
    public class UserCreateCommand : ICommand, ICompanyCommand
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }

    public class DivisionCreateCommand : ICommand, ICompanyCommand
    {
        public string Name { get; set; }
        public string LevelId { get; set; }

        public string CommandCompanyId { get; set; }
        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}