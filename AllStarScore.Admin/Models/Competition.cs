using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllStarScore.Admin.Models
{
    public partial class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
    }

    public partial class Competition
    {
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