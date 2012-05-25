using System;
using System.ComponentModel.DataAnnotations;
using AllStarScore.Admin.Infrastructure.Commands;

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

    public class GymCreateSuccessfulViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public GymCreateSuccessfulViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}