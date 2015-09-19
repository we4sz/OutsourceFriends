using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class GuideSearchViewModel
    {

        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }

        public List<string> ids { get; set; }

        
        public SearchSort? Sort { get; set; }

        public int? MaxBudget { get; set; }
    }
    public class GuideRatingViewModel
    {
        public int Rating { get; set; }

        public string Description { get; set; }

        public TravelerViewModel Traveler { get; set; }

        public DateTime? Created { get; set; }
    }


    public class SmallGuideViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

    }
    public class GuideViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; }

        public int Ratings { get; set; }


    }

}