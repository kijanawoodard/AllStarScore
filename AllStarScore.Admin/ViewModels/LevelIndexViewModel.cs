using System.Collections.Generic;
using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class LevelIndexViewModel
    {
        public List<Level> Levels { get; set; }

        public LevelIndexViewModel(List<Level> levels)
        {
            Levels = levels;
        }
    }
}