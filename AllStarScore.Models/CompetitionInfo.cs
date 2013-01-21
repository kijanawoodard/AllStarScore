using System;
using System.Collections.Generic;
using AllStarScore.Extensions;

namespace AllStarScore.Models
{
    public class CompetitionInfo
    {
        public string Id { get { return Competition.Id; } }
		public Decimal FirstPerformancePercentage { get; set; }

		public Competition Competition { get; set; }
		public Schedule Schedule { get; set; }
		public Company Company { get; set; }
		public CompetitionDivisions CompetitionDivisions { get; set; }
		public List<Level> Levels { get; set; }
		public List<Division> Divisions { get; set; }
		public List<Gym> Gyms { get; set; }
		public List<Registration> Registrations { get; set; }

		public CompetitionInfo()
    	{
    		FirstPerformancePercentage = 0.3M;
    	}
    }
}