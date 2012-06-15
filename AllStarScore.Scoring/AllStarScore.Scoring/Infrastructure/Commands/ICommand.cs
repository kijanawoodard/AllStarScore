using System;

namespace AllStarScore.Scoring.Infrastructure.Commands
{
    public interface ICommand
    {
        string CommandByUser { get; set; }
        DateTime CommandWhen { get; set; }
    }
}
