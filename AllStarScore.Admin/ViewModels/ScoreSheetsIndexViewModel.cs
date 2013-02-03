using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
	public class ScoreSheetsIndexViewModel
	{
		public Competition Competition { get; set; }
		public ScoreSheetsIndexViewModel(Competition competition)
		{
			Competition = competition;
		}
	}
}