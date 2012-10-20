using System;

namespace AllStarScore.Models.Commands
{
	public class DivisionCreateCommand : ICommand, ICompanyCommand
	{
		public string Name { get; set; }
		public string LevelId { get; set; }
		public string ScoringDefinition { get; set; }

		public string CommandCompanyId { get; set; }
		public string CommandByUser { get; set; }
		public DateTime CommandWhen { get; set; }

		public override string ToString()
		{
			return string.Format("LevelId: {0}, Name: {1}", LevelId, Name);
		}
	}
}