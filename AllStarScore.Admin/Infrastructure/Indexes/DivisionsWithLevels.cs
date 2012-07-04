using System.Linq;
using AllStarScore.Models;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class DivisionsWithLevels : AbstractIndexCreationTask<Division>
    {
        public DivisionsWithLevels()
        {
            Map = divisions => from division in divisions
                               select new {division.LevelId};

            TransformResults =
                (database, divisions) => from division in divisions
                                         let level = database.Load<Level>(division.LevelId)
                                         select new {DivisionId = division.Id, Division = division.Name, Level = level.Name};
        }
    }
}