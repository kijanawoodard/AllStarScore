namespace AllStarScore.Models
{
	public class Synchronization : IBelongToCompany
	{
		public static string FormatId(string companyId)
		{
			return companyId + "/synchronization/security";
		}

		public string Id { get { return FormatId(CompanyId); } }
		public string CompanyId { get; set; }

		public string Token { get; set; }
		public string Url { get; set; }
	}
}