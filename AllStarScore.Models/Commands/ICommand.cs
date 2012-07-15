using System;

namespace AllStarScore.Models.Commands
{
    public interface ICommand
    {
        string CommandByUser { get; set; }
        DateTime CommandWhen { get; set; }
    }
}
