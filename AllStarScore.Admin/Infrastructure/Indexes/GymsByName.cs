using System.Linq;
using AllStarScore.Admin.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class GymsByName : AbstractIndexCreationTask<Gym>
    {
        public GymsByName()
        {
            Map = users => from user in users
                           select new { user.Name };

            Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
        }
    }
}