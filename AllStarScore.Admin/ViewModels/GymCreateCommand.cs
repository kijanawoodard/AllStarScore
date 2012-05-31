using System;
using System.ComponentModel.DataAnnotations;
using AllStarScore.Admin.Infrastructure.Commands;
using AllStarScore.Admin.Models;

namespace AllStarScore.Admin.ViewModels
{
    public class GymCreateCommand : ICommand 
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }

        public GymCreateCommand()
        {
            IsSmallGym = true;
        }
    }

    public class GymEditCommand : ICommand
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

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
            Id = gym.Id;
            Name = gym.Name;
            Location = gym.Location;
            IsSmallGym = gym.IsSmallGym;
        }
    }

    public class GymCreateSuccessfulViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public GymCreateSuccessfulViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class GymEditSuccessfulViewModel
    {
        public string Id { get; set; }

        public GymEditSuccessfulViewModel(string id)
        {
            Id = id;
        }
    }
    public class GymDetailsViewModel
    {
        public Gym Gym { get; set; }

        public GymDetailsViewModel(Gym gym)
        {
            Gym = gym;
        }
    }
}