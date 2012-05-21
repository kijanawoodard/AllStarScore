using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllStarScore.Admin.Infrastructure.Commands
{
    public interface ICommand
    {
        string CommandByUser { get; set; }
        DateTime CommandWhen { get; set; }
    }
}
