using System;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class SchedulingEditCommand : ICommand
    {
        public Schedule Schedule { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}