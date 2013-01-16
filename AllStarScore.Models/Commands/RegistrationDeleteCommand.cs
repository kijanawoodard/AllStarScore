using System;
using System.ComponentModel.DataAnnotations;

namespace AllStarScore.Models.Commands
{
	public class RegistrationDeleteCommand : ICommand
	{
		[Required]
		public string Id { get; set; }

		public string CommandByUser { get; set; }
		public DateTime CommandWhen { get; set; }
	}
}