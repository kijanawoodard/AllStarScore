using System;
using System.ComponentModel.DataAnnotations;
using AllStarScore.Admin.Infrastructure.Commands;

namespace AllStarScore.Admin.ViewModels
{
    public class GymCreateInputModel : ICommand 
    {
        public GymCreateInputModel()
        {
            IsSmallGym = true;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }
        public bool IsSmallGym { get; set; }

        public string CommandByUser { get; set; }
        public DateTime CommandWhen { get; set; }
    }
}