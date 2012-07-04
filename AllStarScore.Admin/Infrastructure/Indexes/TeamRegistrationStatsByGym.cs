using System.Linq;
using AllStarScore.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class TeamRegistrationStatsByGym : AbstractIndexCreationTask<TeamRegistration, TeamRegistrationStatsByGym.Results>
    {
        public class Results
        {
            public string CompetitionId { get; set; }
            public string GymId { get; set; }
            public string GymName { get; set; }
            public int TeamCount { get; set; }
            public int ParticipantCount { get; set; }
        }

        public TeamRegistrationStatsByGym()
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
}