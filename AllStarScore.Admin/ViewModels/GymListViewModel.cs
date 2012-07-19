using System.Collections.Generic;
using AllStarScore.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class GymListViewModel
    {
        public List<Gym> Gyms { get; set; }

        public GymListViewModel(List<Gym> gyms)
        {
            Gyms = gyms;
        }
    }
}