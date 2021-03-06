using System.Collections.Generic;

namespace AllStarScore.Models
{
	public class ScoringMap
	{
		public Dictionary<string, string> ScoreSheets
		{
			get
			{
				return new Dictionary<string, string>()
				{
					{"scoring-level1", "levels-level1-template"},
					{"scoring-level2", "levels-level2-template"},
					{"scoring-level3", "levels-level3-template"},
					{"scoring-level4", "levels-level4-template"},
					{"scoring-division42", "division-42-template"},
					{"scoring-level5", "levels-level5-template"},
					{"scoring-restricted5", "restricted5-template"},
					{"scoring-level6", "levels-level6-template"},
					{"scoring-alternative-all-star", "levels-alternative-all-star-template"},

					{"scoring-school", "levels-school-template"},
					{"scoring-dance", "levels-dance-template"},
					{"scoring-hiphop", "levels-hiphop-template"},
//						   
//						   {"scoring-dance-jazz", "division-jazz-template"},
//                           {"scoring-dance-hiphop", "division-hiphop-template"},

					{"judges-deductions", "judges-deductions-template"},
					{"judges-legalities", "judges-legalities-template"}
				};
			}
		}

		public Dictionary<string, string> Templates
		{
			get
			{
				return new Dictionary<string, string>()
				{
					{"scoring-level1", "all_star_template"},
					{"scoring-level2", "all_star_template"},
					{"scoring-level3", "all_star_template"},
					{"scoring-level4", "all_star_template"},
					{"scoring-division42", "all_star_template"},
					{"scoring-level5", "all_star_template"},
					{"scoring-restricted5", "all_star_template"},
					{"scoring-level6", "all_star_template"},
					{"scoring-alternative-all-star", "all_star_template"},
					{"scoring-school", "single_column_template"},
					{"scoring-dance", "single_column_template"},
					{"scoring-hiphop", "single_column_template"},
                           
					{"judges-deductions", "single_value_template"},
					{"judges-legalities", "single_value_template"}
				};
			}
		}

		public Dictionary<string, IScoringDefinition> Categories
		{
			get
			{
				return new Dictionary<string, IScoringDefinition>()
				{
					{"scoring-level1", new Level1ScoringDefinition()},
					{"scoring-level2", new Level2ScoringDefinition()},
					{"scoring-level3", new Level3ScoringDefinition()},
					{"scoring-level4", new Level4ScoringDefinition()},
					{"scoring-division42", new Level42ScoringDefinition()},
					{"scoring-level5", new Level5ScoringDefinition()},
					{"scoring-restricted5", new Level5SeniorRestrictedScoringDefinition()},
					{"scoring-level6", new Level6ScoringDefinition()},
					{"scoring-alternative-all-star", new AlternativeAllStarScoringDefinition()},
					{"scoring-school", new SchoolScoringDefinition()},
						   
					{"scoring-dance", new DanceOpenScoringDefinition()},

//						   {"scoring-dance-open", new DanceOpenScoringDefinition()},
//						   {"scoring-dance-jazz", new DanceJazzScoringDefinition()},
//						   {"scoring-dance-pom", new DancePomScoringDefinition()},
//						   {"scoring-dance-prop", new DanceOpenScoringDefinition()},
//						   {"scoring-dance-novelty", new DanceOpenScoringDefinition()},
//						   {"scoring-dance-ballet", new DanceOpenScoringDefinition()},
//						   {"scoring-dance-lyrical", new DanceOpenScoringDefinition()},

					{"scoring-hiphop", new DanceHipHopScoringDefinition()},
//						   {"scoring-dance-hiphop", new DanceHipHopScoringDefinition()},
//						   {"scoring-dance-crew", new DanceHipHopScoringDefinition()},
//						   
//						   {"scoring-dance-variety", new DanceVarietyScoringDefinition()},
                           
					{"judges-deductions", new DeductionsScoringDefinition()},
					{"judges-legalities", new LegalitiesScoringDefinition()}
				};
			}
		}
	}

	public interface IScoringDefinition { }

	//why decimal? was getting some friction from Raven, plus http://stackoverflow.com/a/1165788/214073
	public class ScoreEntry
	{
		public decimal Base { get; set; }
		public decimal Execution { get; set; }

		public decimal Total { get { return (Base + Execution); } }
	}

	public class ScoringCategory
	{
		public string Display { get; set; }
		public float Min { get; set; }
		public float Max { get; set; }
		public bool IncludeExectionScore { get; set; }
	}

	public abstract class AllStarScoringDefinition : IScoringDefinition
	{
		public ScoringCategory Stunts { get; set; }
		public ScoringCategory Pyramids { get; set; }
		public ScoringCategory Tosses { get; set; }
		public ScoringCategory StandardTumbling { get; set; }
		public ScoringCategory RunningTumbling { get; set; }
		public ScoringCategory Jumps { get; set; }
		public ScoringCategory MotionsDance { get; set; }
		public ScoringCategory FormationsTransitions { get; set; }
		public ScoringCategory PerformanceCategory { get; set; }
		public ScoringCategory SkillsCreativity { get; set; }
		public ScoringCategory RoutineCreativity { get; set; }

		protected AllStarScoringDefinition(int min, int max)
		{
			Stunts = new ScoringCategory { Display = "Stunts", Min = min, Max = max, IncludeExectionScore = true };
			Pyramids = new ScoringCategory { Display = "Pyramids", Min = min, Max = max, IncludeExectionScore = true };
			Tosses = new ScoringCategory { Display = "Tosses", Min = min, Max = max, IncludeExectionScore = true };
			StandardTumbling = new ScoringCategory { Display = "Standard Tumbling", Min = min, Max = max, IncludeExectionScore = true };
			RunningTumbling = new ScoringCategory { Display = "Running Tumbling", Min = min, Max = max, IncludeExectionScore = true };
			Jumps = new ScoringCategory { Display = "Jumps", Min = min, Max = max, IncludeExectionScore = true };
			MotionsDance = new ScoringCategory { Display = "Motions / Dance", Min = min, Max = max, IncludeExectionScore = true };
			FormationsTransitions = new ScoringCategory { Display = "Formations / Transitions", Min = min, Max = max, IncludeExectionScore = false };
			PerformanceCategory = new ScoringCategory { Display = "Performance", Min = min, Max = max, IncludeExectionScore = false };
			SkillsCreativity = new ScoringCategory { Display = "Skills Creativity", Min = 0, Max = 5, IncludeExectionScore = false };
			RoutineCreativity = new ScoringCategory { Display = "Routine Creativity", Min = 0, Max = 5, IncludeExectionScore = false };
		}
	}

	public class Level1ScoringDefinition : AllStarScoringDefinition
	{
		public Level1ScoringDefinition() : base(3, 5) { }
	}

	public class Level2ScoringDefinition : AllStarScoringDefinition
	{
		public Level2ScoringDefinition() : base(4, 6) { }
	}

	public class Level3ScoringDefinition : AllStarScoringDefinition
	{
		public Level3ScoringDefinition() : base(5, 7) { }
	}

	public class Level4ScoringDefinition : AllStarScoringDefinition
	{
		public Level4ScoringDefinition() : base(6, 8) { }
	}

	public class Level42ScoringDefinition : AllStarScoringDefinition
	{
		public Level42ScoringDefinition()
			: base(6, 8)
		{
			StandardTumbling.Min = 4;
			StandardTumbling.Max = 6;

			RunningTumbling.Min = 4;
			RunningTumbling.Max = 6;
		}
	}

	public class Level5ScoringDefinition : AllStarScoringDefinition
	{
		public Level5ScoringDefinition() : base(8, 10) { }
	}

	public class Level5SeniorRestrictedScoringDefinition : Level5ScoringDefinition
	{
		public Level5SeniorRestrictedScoringDefinition()
		{
			RunningTumbling.Max = 9;
		}
	}

	public class Level6ScoringDefinition : AllStarScoringDefinition
	{
		public Level6ScoringDefinition()
			: base(8, 10)
		{
			Stunts.Max = 11;
			Pyramids.Max = 11;
			Tosses.Max = 11;
		}
	}

	public class AlternativeAllStarScoringDefinition : AllStarScoringDefinition
	{
		public AlternativeAllStarScoringDefinition() : base(5, 10) { }
	}

	public class DeductionsScoringDefinition : IScoringDefinition
	{
		public ScoringCategory Deductions { get; set; }

		public DeductionsScoringDefinition()
		{
			Deductions = new ScoringCategory() { Display = "Deductions", Min = 0, Max = 20, IncludeExectionScore = false };
		}
	}

	public class LegalitiesScoringDefinition : IScoringDefinition
	{
		public ScoringCategory Legalities { get; set; }

		public LegalitiesScoringDefinition()
		{
			Legalities = new ScoringCategory() { Display = "Legalities", Min = 0, Max = 20, IncludeExectionScore = false };
		}
	}

	public class SchoolScoringDefinition : IScoringDefinition
	{
		public ScoringCategory Stunts { get; set; }
		public ScoringCategory PyramidsTosses { get; set; }
		public ScoringCategory Tumbling { get; set; }
		public ScoringCategory Jumps { get; set; }
		public ScoringCategory MotionsDance { get; set; }
		public ScoringCategory Timing { get; set; }
		public ScoringCategory Transitions { get; set; }
		public ScoringCategory PerformanceCategory { get; set; }
		public ScoringCategory SchoolRepresentation { get; set; }
		public ScoringCategory OverallImpression { get; set; }

		public SchoolScoringDefinition()
		{
			Stunts = new ScoringCategory { Display = "Stunts", Min = 0, Max = 10, IncludeExectionScore = false };
			PyramidsTosses = new ScoringCategory { Display = "Pyramids / Tosses", Min = 0, Max = 10, IncludeExectionScore = false };
			Tumbling = new ScoringCategory { Display = "Tumbling", Min = 0, Max = 10, IncludeExectionScore = false };
			Timing = new ScoringCategory { Display = "Timing", Min = 0, Max = 10, IncludeExectionScore = false };
			Jumps = new ScoringCategory { Display = "Jumps", Min = 0, Max = 10, IncludeExectionScore = false };
			MotionsDance = new ScoringCategory { Display = "Motions / Dance", Min = 0, Max = 10, IncludeExectionScore = false };
			Transitions = new ScoringCategory { Display = "Transitions", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceCategory = new ScoringCategory { Display = "Performance", Min = 0, Max = 10, IncludeExectionScore = false };
			SchoolRepresentation = new ScoringCategory { Display = "School Representation", Min = 0, Max = 10, IncludeExectionScore = false };
			OverallImpression = new ScoringCategory { Display = "Overall Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}

	public class DanceOpenScoringDefinition : IScoringDefinition
	{
		public ScoringCategory RoutineExecution { get; set; }
		public ScoringCategory TechnicalSkills { get; set; }
		public ScoringCategory VisualEffects { get; set; }
		public ScoringCategory Staging { get; set; }
		public ScoringCategory DegreeOfDifficulty { get; set; }
		public ScoringCategory Uniformity { get; set; }
		public ScoringCategory Choreography { get; set; }
		public ScoringCategory PerformanceImpression { get; set; }

		public DanceOpenScoringDefinition()
		{
			RoutineExecution = new ScoringCategory { Display = "Routine Execution / Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			TechnicalSkills = new ScoringCategory { Display = "Technical Skills / Turn", Min = 0, Max = 10, IncludeExectionScore = false };
			VisualEffects = new ScoringCategory { Display = "Visual Effects / Leap", Min = 0, Max = 10, IncludeExectionScore = false };
			Staging = new ScoringCategory { Display = "Staging", Min = 0, Max = 10, IncludeExectionScore = false };
			DegreeOfDifficulty = new ScoringCategory { Display = "Difficulty", Min = 0, Max = 10, IncludeExectionScore = false };
			Uniformity = new ScoringCategory { Display = "Uniformity", Min = 0, Max = 10, IncludeExectionScore = false };
			Choreography = new ScoringCategory { Display = "Choreography", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceImpression = new ScoringCategory { Display = "Performance Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}

	public class DanceJazzScoringDefinition : IScoringDefinition
	{
		public ScoringCategory JazzTechnique { get; set; }
		public ScoringCategory TurnTechnique { get; set; }
		public ScoringCategory LeapTechnique { get; set; }
		public ScoringCategory Staging { get; set; }
		public ScoringCategory DegreeOfDifficulty { get; set; }
		public ScoringCategory Uniformity { get; set; }
		public ScoringCategory Choreography { get; set; }
		public ScoringCategory PerformanceImpression { get; set; }

		public DanceJazzScoringDefinition()
		{
			JazzTechnique = new ScoringCategory { Display = "Jazz Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			TurnTechnique = new ScoringCategory { Display = "Turn Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			LeapTechnique = new ScoringCategory { Display = "Leap Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			Staging = new ScoringCategory { Display = "Staging", Min = 0, Max = 10, IncludeExectionScore = false };
			DegreeOfDifficulty = new ScoringCategory { Display = "Difficulty", Min = 0, Max = 10, IncludeExectionScore = false };
			Uniformity = new ScoringCategory { Display = "Uniformity", Min = 0, Max = 10, IncludeExectionScore = false };
			Choreography = new ScoringCategory { Display = "Choreography", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceImpression = new ScoringCategory { Display = "Performance Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}

	public class DancePomScoringDefinition : IScoringDefinition
	{
		public ScoringCategory PomTechnique { get; set; }
		public ScoringCategory TechnicalSkills { get; set; }
		public ScoringCategory VisualEffects { get; set; }
		public ScoringCategory Staging { get; set; }
		public ScoringCategory DegreeOfDifficulty { get; set; }
		public ScoringCategory Uniformity { get; set; }
		public ScoringCategory Choreography { get; set; }
		public ScoringCategory PerformanceImpression { get; set; }

		public DancePomScoringDefinition()
		{
			PomTechnique = new ScoringCategory { Display = "Pom Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			TechnicalSkills = new ScoringCategory { Display = "Technical Skills", Min = 0, Max = 10, IncludeExectionScore = false };
			VisualEffects = new ScoringCategory { Display = "Visual Effects", Min = 0, Max = 10, IncludeExectionScore = false };
			Staging = new ScoringCategory { Display = "Staging", Min = 0, Max = 10, IncludeExectionScore = false };
			DegreeOfDifficulty = new ScoringCategory { Display = "Difficulty", Min = 0, Max = 10, IncludeExectionScore = false };
			Uniformity = new ScoringCategory { Display = "Uniformity", Min = 0, Max = 10, IncludeExectionScore = false };
			Choreography = new ScoringCategory { Display = "Choreography", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceImpression = new ScoringCategory { Display = "Performance Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}

	public class DanceHipHopScoringDefinition : IScoringDefinition
	{
		public ScoringCategory HipHopTechnique { get; set; }
		public ScoringCategory VisualEffects { get; set; }
		public ScoringCategory Staging { get; set; }
		public ScoringCategory DegreeOfDifficulty { get; set; }
		public ScoringCategory Uniformity { get; set; }
		public ScoringCategory Choreography { get; set; }
		public ScoringCategory PerformanceImpression { get; set; }

		public DanceHipHopScoringDefinition()
		{
			HipHopTechnique = new ScoringCategory { Display = "Hip Hop Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			VisualEffects = new ScoringCategory { Display = "Visual Effects", Min = 0, Max = 10, IncludeExectionScore = false };
			Staging = new ScoringCategory { Display = "Staging", Min = 0, Max = 10, IncludeExectionScore = false };
			DegreeOfDifficulty = new ScoringCategory { Display = "Difficulty", Min = 0, Max = 10, IncludeExectionScore = false };
			Uniformity = new ScoringCategory { Display = "Uniformity", Min = 0, Max = 10, IncludeExectionScore = false };
			Choreography = new ScoringCategory { Display = "Choreography", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceImpression = new ScoringCategory { Display = "Performance Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}

	public class DanceVarietyScoringDefinition : IScoringDefinition
	{
		public ScoringCategory JazzTechnique { get; set; }
		public ScoringCategory PomTechnique { get; set; }
		public ScoringCategory HipHopTechnique { get; set; }
		public ScoringCategory TechnicalSkills { get; set; }
		public ScoringCategory Staging { get; set; }
		public ScoringCategory DegreeOfDifficulty { get; set; }
		public ScoringCategory Uniformity { get; set; }
		public ScoringCategory Choreography { get; set; }
		public ScoringCategory PerformanceImpression { get; set; }

		public DanceVarietyScoringDefinition()
		{
			JazzTechnique = new ScoringCategory { Display = "Jazz Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			PomTechnique = new ScoringCategory { Display = "Pom Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			HipHopTechnique = new ScoringCategory { Display = "Hip Hop Technique", Min = 0, Max = 10, IncludeExectionScore = false };
			TechnicalSkills = new ScoringCategory { Display = "Technical Skills", Min = 0, Max = 10, IncludeExectionScore = false };
			Staging = new ScoringCategory { Display = "Staging", Min = 0, Max = 10, IncludeExectionScore = false };
			DegreeOfDifficulty = new ScoringCategory { Display = "Difficulty", Min = 0, Max = 10, IncludeExectionScore = false };
			Uniformity = new ScoringCategory { Display = "Uniformity", Min = 0, Max = 10, IncludeExectionScore = false };
			Choreography = new ScoringCategory { Display = "Choreography", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceImpression = new ScoringCategory { Display = "Performance Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}

	public class IndividualBestDancerScoringDefinition : IScoringDefinition
	{
		public ScoringCategory Execution { get; set; }
		public ScoringCategory TechnicalSkills { get; set; }
		public ScoringCategory DegreeOfDifficulty { get; set; }
		public ScoringCategory Choreography { get; set; }
		public ScoringCategory PerformanceImpression { get; set; }

		public IndividualBestDancerScoringDefinition()
		{
			Execution = new ScoringCategory { Display = "Execution", Min = 0, Max = 10, IncludeExectionScore = false };
			TechnicalSkills = new ScoringCategory { Display = "Technical Skills", Min = 0, Max = 10, IncludeExectionScore = false };
			DegreeOfDifficulty = new ScoringCategory { Display = "Difficulty", Min = 0, Max = 10, IncludeExectionScore = false };
			Choreography = new ScoringCategory { Display = "Choreography", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceImpression = new ScoringCategory { Display = "Performance Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}

	public class IndividualBestGroupScoringDefinition : IScoringDefinition
	{
		public ScoringCategory Execution { get; set; }
		public ScoringCategory Staging { get; set; }
		public ScoringCategory DegreeOfDifficulty { get; set; }
		public ScoringCategory Choreography { get; set; }
		public ScoringCategory PerformanceImpression { get; set; }

		public IndividualBestGroupScoringDefinition()
		{
			Execution = new ScoringCategory { Display = "Execution", Min = 0, Max = 10, IncludeExectionScore = false };
			Staging = new ScoringCategory { Display = "Staging", Min = 0, Max = 10, IncludeExectionScore = false };
			DegreeOfDifficulty = new ScoringCategory { Display = "Difficulty", Min = 0, Max = 10, IncludeExectionScore = false };
			Choreography = new ScoringCategory { Display = "Choreography", Min = 0, Max = 10, IncludeExectionScore = false };
			PerformanceImpression = new ScoringCategory { Display = "Performance Impression", Min = 0, Max = 10, IncludeExectionScore = false };
		}
	}
}