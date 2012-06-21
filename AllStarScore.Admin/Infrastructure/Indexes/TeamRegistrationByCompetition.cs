using System;
using System.Globalization;
using System.Linq;
using AllStarScore.Admin.Models;
using AllStarScore.Models;
using Raven.Abstractions.Indexing;
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
            public bool IsShowTeam { get; set; }
            public string GymName { get; set; }
            public string GymLocation { get; set; }
            public bool IsSmallGym { get; set; }
            public string DivisionName { get; set; }
            public string LevelName { get; set; }

            public DateTime RegistrationDate { get; set; }
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
                                           registration.DivisionId,
                                           RegistrationDate = registration.History.First().CommandWhen
                                       };
            
            TransformResults =
                (database, registrations) => from registration in registrations
                                             let tr = database.Load<TeamRegistration>(registration.Id)
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
                                                        RegistrationDate = tr.History.First().CommandWhen
                                                    };
        }
    }
}