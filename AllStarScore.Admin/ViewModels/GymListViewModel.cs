using System.Collections.Generic;
using AllStarScore.Admin.Infrastructure.Indexes;

namespace AllStarScore.Admin.ViewModels
{
    public class GymListViewModel
    {
        public List<GymsByName.Results> Gyms { get; set; }

        public GymListViewModel(List<GymsByName.Results> gyms)
        {
            Gyms = gyms;
        }
    }
}