using System.Linq;
using AllStarScore.Admin.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class TeamRegistrationByGym : AbstractIndexCreationTask<TeamRegistration, TeamRegistrationByGym.Results>
    {
        public class Results
        {
            public string CompetitionId { get; set; }
            public string GymId { get; set; }
            public string GymName { get; set; }
            public int TeamCount { get; set; }
            public int ParticipantCount { get; set; }
        }

        public TeamRegistrationByGym()
        {
            Map = registrations => from registration in registrations
                                   select
                                       new
                                           {
                                               registration.CompetitionId,
                                               registration.GymId,
                                               registration.ParticipantCount,
                                               TeamCount = 1
                                           };

            Reduce = registrations => from registration in registrations
                                      group registration by new {registration.CompetitionId, registration.GymId}
                                      into g
                                      select new
                                                 {
                                                     g.Key.CompetitionId,
                                                     g.Key.GymId,
                                                     ParticipantCount = g.Sum(x => x.ParticipantCount),
                                                     TeamCount = g.Sum(x => x.TeamCount)
                                                 };

            TransformResults =
                (database, registrations) => from registration in registrations
                                             let gym = database.Load<Gym>(registration.GymId)
                                             select new
                                                        {
                                                            registration.CompetitionId,
                                                            GymName = gym.Name,
                                                            registration.GymId,
                                                            registration.ParticipantCount,
                                                            registration.TeamCount
                                                        };
        }
    }

    public class TeamRegistrationByCompetition : AbstractIndexCreationTask<TeamRegistration, TeamRegistrationByCompetition.Results>
    {
        public class Results
        {
            public string CompetitionId { get; set; }
            public int GymCount { get; set; }
            public int TeamCount { get; set; }
            public int ParticipantCount { get; set; }
        }

        public TeamRegistrationByCompetition()
        {
            Map = registrations => from registration in registrations
                                   select
                                       new
                                       {
                                           registration.CompetitionId,
                                           registration.GymId,
                                           registration.ParticipantCount,
                                           GymCount = 1,
                                           TeamCount = 1
                                       };

            Reduce = registrations => from registration in registrations
                                      group registration by new { registration.CompetitionId }
                                          into g
                                          select new
                                          {
                                              g.Key.CompetitionId,
                                              GymCount = g.Sum(x => x.g),
                                              ParticipantCount = g.Sum(x => x.ParticipantCount),
                                              TeamCount = g.Sum(x => x.TeamCount)
                                          };

            TransformResults =
                (database, registrations) => from registration in registrations
                                             let gym = database.Load<Gym>(registration.GymId)
                                             select new
                                             {
                                                 registration.CompetitionId,
                                                 GymName = gym.Name,
                                                 registration.GymId,
                                                 registration.ParticipantCount,
                                                 registration.TeamCount
                                             };
        }
    }
}