using System.Collections.Generic;
using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
	public class CompetitionDataIndexViewModel
	{
		public Schedule Schedule { get; set; }
		public Competition Competition { get; set; }

		public IEnumerable<Level> Levels { get; set; }
		public IEnumerable<Division> Divisions { get; set; }
		public IEnumerable<Registration> Registrations { get; set; }
		public IEnumerable<Gym> Gyms { get; set; }
		public IEnumerable<Performance> Performances { get; set; }

		public CompetitionDataIndexViewModel(Schedule schedule
		                                     , Competition competition
		                                     , IEnumerable<Level> levels
		                                     , IEnumerable<Division> divisions
		                                     , IEnumerable<Gym> gyms
		                                     , IEnumerable<Registration> registrations
		                                     , IEnumerable<Performance> performances
			)
		{
			Schedule = schedule;
			Competition = competition;

			Levels = levels;
			Divisions = divisions;
			Gyms = gyms;
			Registrations = registrations;
			Performances = performances;
		}
	}
}