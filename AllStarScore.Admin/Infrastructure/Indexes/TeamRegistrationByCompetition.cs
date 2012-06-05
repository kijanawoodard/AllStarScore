using System.Linq;
using AllStarScore.Admin.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class TeamRegistrationByCompetition : AbstractIndexCreationTask<TeamRegistration, TeamRegistrationByCompetition.Results>
    {
        public class Results
        {
            public string Id { get; set; }
            public string CompetitionId { get; set; }
            public string GymId { get; set; }
            public string DivisionId { get; set; }
            public string LevelId { get; set; }
            
            public string TeamName { get; set; }
            public int ParticipantCount { get; set; }
            public string GymName { get; set; }
            public string GymLocation { get; set; }
            public string DivisionName { get; set; }
            public string LevelName { get; set; }
        }

        public TeamRegistrationByCompetition()
        {
            Map = registrations => from registration in registrations
                                   select
                                       new
                                       {
                                           registration.Id, 
                                           registration.CompetitionId,
                                           registration.GymId,
                                           registration.DivisionId
                                       };
            
            TransformResults =
                (database, registrations) => from registration in registrations
                                             let gym = database.Load<Gym>(registration.GymId)
                                             let division = database.Load<Division>(registration.DivisionId)
                                             let level = database.Load<Level>(division.LevelId)
                                             select new
                                                    {
                                                        registration.Id,
                                                        registration.CompetitionId,
                                                        registration.GymId,
                                                        registration.TeamName,
                                                        registration.ParticipantCount,
                                                        GymName = gym.Name,
                                                        GymLocation = gym.Location,
                                                        registration.DivisionId,
                                                        division.LevelId,
                                                        DivisionName = division.Name,
                                                        LevelName = level.Name
                                                    };
        }
    }
}