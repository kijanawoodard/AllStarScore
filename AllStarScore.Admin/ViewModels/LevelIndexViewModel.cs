using System.Collections.Generic;
using System.Linq;
using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class LevelIndexViewModel
    {
	    public CompetitionDivisions Info { get; set; }
	    public Dictionary<string, string> Levels { get; set; }
		public Dictionary<string, List<string>> Divisions { get; set; }

        public LevelIndexViewModel(CompetitionDivisions info)
        {
	        Info = info;
	        Levels = info.Levels.ToDictionary(x => x.Id, x => x.Name);
	        Divisions = info.Divisions.GroupBy(x => x.LevelId).ToDictionary(x => x.Key, x => x.Select(d => d.Name).ToList());
        }
    }
}