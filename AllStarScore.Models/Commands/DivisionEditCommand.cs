using System;

namespace AllStarScore.Models.Commands
{
	public class DivisionEditCommand : ICommand, ICompanyCommand
	{
		public string Id { get; set; }
		public string Name { get; set; }
		
		public string CommandCompanyId { get; set; }
		public string CommandByUser { get; set; }
		public DateTime CommandWhen { get; set; }
	}
}