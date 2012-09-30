namespace AllStarScore.Admin.ViewModels
{
    public class SuperAdminIndexViewModel
    {
        public string Company { get; set; }
        public string Domain { get; set; }
		public string Token { get; set; }

        public bool ThereIsNoCompanyForThisDomain { get { return string.IsNullOrWhiteSpace(Company); } }
    }
}