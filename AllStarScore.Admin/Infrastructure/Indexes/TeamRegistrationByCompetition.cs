using System;
using System.Globalization;
using System.Linq;
using AllStarScore.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class TeamRegistrationByCompetition : AbstractIndexCreationTask<Registration, TeamRegistrationByCompetitionResults>
    {
        

        public TeamRegistrationByCompetition()
        {
            Map = registrations => from registration in registrations
                                   select
                                       new
                                       {
                                           registration.Id, 
                                           registration.CompetitionId,
                                           registration.GymId,
                                           registration.DivisionId,
                                           registration.CreatedAt,
                                       };
            
            TransformResults =
                (database, registrations) => from registration in registrations
                                             let tr = database.Load<Registration>(registration.Id)
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
                                                        registration.IsShowTeam,
                                                        GymName = gym.Name,
                                                        GymLocation = gym.Location,
                                                        gym.IsSmallGym,
                                                        registration.DivisionId,
                                                        division.LevelId,
                                                        DivisionName = division.Name,
                                                        LevelName = level.Name,
                                                        registration.CreatedAt
                                                    };
        }
    }
}