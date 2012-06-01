using System.Linq;
using AllStarScore.Admin.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace AllStarScore.Admin.Infrastructure.Indexes
{
    public class GymsByName : AbstractIndexCreationTask<Gym>
    {
        public class Results
        {
            public string GymId { get; set; }
            public string GymName { get; set; }
            public string Location { get; set; }
            public bool IsSmallGym { get; set; }
        }

        public GymsByName()
        {
            Map = gyms => from gym in gyms
                           select new { gym.Name, gym.Location, gym.Id };

            
            Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
            Indexes.Add(x => x.Location, FieldIndexing.Analyzed);

            TransformResults =
                (database, gyms) => from gym in gyms
                                             select new
                                             {
                                                 GymId = gym.Id,
                                                 GymName = gym.Name,
                                                 gym.Location,
                                                 gym.IsSmallGym
                                             };
        }
    }
}