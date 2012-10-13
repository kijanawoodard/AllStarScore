using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
	public class CompanyNameViewModel
	{
		public Company Company { get; set; }

		public CompanyNameViewModel(Company company)
		{
			Company = company;
		}
	}
}