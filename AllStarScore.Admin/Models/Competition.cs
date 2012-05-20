using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AllStarScore.Admin.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime FirstDay { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastDay { get; set; }
    
        public override bool Equals(object obj)
        {
            var target = obj as Competition;
            if (target == null) return false;
                
           return Id.Equals(target.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}