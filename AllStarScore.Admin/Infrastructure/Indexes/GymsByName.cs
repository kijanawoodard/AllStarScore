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
            Map = gyms => from gym in gyms
                           select new { gym.Name, gym.Location, gym.Id };

            
            Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
            Indexes.Add(x => x.Location, FieldIndexing.Analyzed);
        }
    }
}