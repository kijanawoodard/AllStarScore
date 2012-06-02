using System;
using System.ComponentModel.DataAnnotations;
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
            public string CompetitionName { get; set; }

            [DataType(DataType.Date)]
            public DateTime CompetitionFirstDay { get; set; }

            public string GymId { get; set; } //not used; here as a placeholder for the map reduce; see SO link below

            public int GymCount { get; set; }
            public int TeamCount { get; set; }
            public int ParticipantCount { get; set; }

            public override bool Equals(object obj)
            {
                var target = obj as Results;
                if (target == null) return false;

                return CompetitionId.Equals(target.CompetitionId);
            }

            public override int GetHashCode()
            {
                return CompetitionId.GetHashCode();
            }

            public override string ToString()
            {
                return CompetitionName;
            }
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
            //http://stackoverflow.com/a/10597740/214073
            Reduce = registrations => from registration in registrations
                                      group registration by new { registration.CompetitionId }
                                          into g
                                          select new
                                          {
                                              g.Key.CompetitionId,
                                              GymId = g.First().GymId,
                                              GymCount = g.Select(x => x.GymId).Distinct().Count(), //from z in g group z by z.GymId into g2 select g2.Sum(x => x.GymCount),
                                              ParticipantCount = g.Sum(x => x.ParticipantCount),
                                              TeamCount = g.Sum(x => x.TeamCount)
                                          };

            TransformResults =
                (database, registrations) => from registration in registrations
                                             let competition = database.Load<Competition>(registration.CompetitionId)
                                             select new
                                             {
                                                 registration.CompetitionId,
                                                 CompetitionName = competition.Name,
                                                 CompetitionFirstDay = competition.FirstDay,
                                                 registration.GymId,
                                                 registration.GymCount,
                                                 registration.ParticipantCount,
                                                 registration.TeamCount
                                             };
        }
    }
}