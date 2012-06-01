using System;
using System.ComponentModel.DataAnnotations;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class GymEditCommand : ICommand
    {
        [Required]
        public string GymId { get; set; }

        [Required]
        public string GymName { get; set; }

        [Required]
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }

        public GymEditCommand()
        {
            
        }

        public GymEditCommand(Gym gym)
        {
            GymId = gym.Id;
            GymName = gym.Name;
            Location = gym.Location;
            IsSmallGym = gym.IsSmallGym;
        }
    }
}