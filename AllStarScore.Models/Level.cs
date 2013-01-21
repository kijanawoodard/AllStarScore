using System;
using System.Collections.Generic;
using System.Linq;

namespace AllStarScore.Models
{
    public class Level : IBelongToCompany
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string ScoringDefinition { get; set; }

        public string CompanyId { get; set; }

        public static string FormatId(string companyId)
        {
            return companyId + "/level/";
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}", Id, Name);
        }

        public bool Equals(Level other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Level)) return false;
            return Equals((Level) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

	public sealed class AwardsLevel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public bool SuppressLevelWinner { get; private set; }

		public List<string> PerformanceLevels { get; set; } 

		private string RankingCalculator { get; set; }

		private const string NaturalRanking = "Natural";
		private void SetNaturalRanking() { RankingCalculator = NaturalRanking; }
		public bool UseNaturalRanking { get { return RankingCalculator == NaturalRanking; } }
		
		private const string SmallGymRanking = "SmallGym";
		private void SetSmallGymRanking() { RankingCalculator = SmallGymRanking; }
		public bool UseSmallGymRanking { get { return RankingCalculator == SmallGymRanking; } }

		public static AwardsLevel CreateAwardsLevel(string id, string name, params string[] levels)
		{
			var result = new AwardsLevel(id, name, levels);
			result.SetNaturalRanking();
			return result;
		}

		public static AwardsLevel CreateAwardsLevelWithoutLevelChampion(string id, string name, params string[] levels)
		{
			var result = CreateAwardsLevel(id, name, levels);
			result.SuppressLevelWinner = true;
			return result;
		}

		public static AwardsLevel CreateAwardsLevelForSmallGyms(string id, string name, params string[] levels)
		{
			var result = new AwardsLevel(id, name, levels);
			result.SetSmallGymRanking();
			return result;
		}

		private AwardsLevel(string id, string name, params string[] levels)
		{
			PerformanceLevels = new List<string>();
			Id = "awards/level/" + id;
			Name = name;
			if (levels == null || levels.Length == 0)
				levels = new[] { id }; //if no levels, then use the id

			PerformanceLevels.AddRange(levels);
		}

		//here for raven/json.net
		public AwardsLevel()
		{
			
		}
	}

	public class CompetitionDivisions : IBelongToCompany
	{
		public string Id { get { return FormatId(CompanyId); } }
		public string CompanyId { get; set; }

		public CompetitionDivisions(string companyId)
		{
			CompanyId = companyId;
		}

		public ICollection<AwardsLevel> AwardsLevels { get; set; }
		public ICollection<Level> Levels { get; set; }
		public ICollection<Division> Divisions { get; set; }

		public static string FormatId(string companyId)
		{
			return companyId + "/config/divisions";
		}

		/// <summary>
		/// Find the awards level given the id of the performance level
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public AwardsLevel FindAwardsLevel(string level)
		{
			var result = AwardsLevels.First(x => x.PerformanceLevels.Contains(level));
			return result;
		}

		public void Validate()
		{
			var awards = AwardsLevels.SelectMany(x => x.PerformanceLevels).ToList();
			var levels = Levels.Select(x => x.Id).ToList();
			var divisionLevels = Divisions.Select(x => x.LevelId).ToList();

			var mistakes = awards.Except(levels).ToList();
			if (mistakes.Any())
				throw new ApplicationException("There are levels in awards that are not actually levels: " + string.Join(", ", mistakes));

			mistakes = levels.Except(awards).ToList();
			if (mistakes.Any())
				throw new ApplicationException("There are levels that don't have awards: " + string.Join(", ", mistakes));

			mistakes = divisionLevels.Except(levels).ToList();
			if (mistakes.Any())
				throw new ApplicationException("There are divisions assigned to levels that are not actually levels: " + string.Join(", ", mistakes));
		}
	}
}