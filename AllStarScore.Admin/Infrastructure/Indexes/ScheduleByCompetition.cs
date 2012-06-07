using System;
using System.Linq;
using AllStarScore.Admin.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class ScheduleByCompetition : AbstractIndexCreationTask<Schedule>
    {
        public ScheduleByCompetition()
        {
            Map = schedules => from schedule in schedules
                                   select
                                       new
                                       {
                                           schedule.Id,
                                           schedule.CompetitionId,
                                       };
        }
    }
}