using System.ComponentModel.DataAnnotations;

namespace AllStarScore.Admin.ViewModels
{
    public class GymCreateInputModel
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
    }
}