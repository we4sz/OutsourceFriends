using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class TravelerViewModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Id { get; set; }

    }

    public class TravelerRatingViewModel
    {
        public int Rating { get; set; }

        public string Description { get; set; }

        public SmallGuideViewModel Guide { get; set; }

        public DateTime? Created { get; set; }
    }

}