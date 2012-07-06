using System;

namespace AllStarScore.Models
{
    public interface ICanBeUpdatedByCommand
    {
        string LastCommand { get; set; }
        string LastCommandBy { get; set; }
        DateTime LastCommandDate { get; set; }    
    }
}