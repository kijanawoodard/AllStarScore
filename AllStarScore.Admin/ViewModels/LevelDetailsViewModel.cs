using System.Collections.Generic;
using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class LevelDetailsViewModel
    {
        public Level Level { get; set; }
        public List<Division> Divisions { get; set; }

        public LevelDetailsViewModel(Level level, List<Division> divisions)
        {
            Level = level;
            Divisions = divisions;
        }
    }
}