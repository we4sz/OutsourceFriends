using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class GuideDescriptionViewModel
    {

        [Required]
        public string Description { get; set; }

    }

    public class GuideTitleViewModel
    {
        [Required]
        public string Title { get; set; }

    }

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

    public class GuideBudgetViewModel
    {

        [Required]
        public int MinBudget { get; set; }

    }


    public class GuideLocationViewModel
    {

        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }

    }

    public class GuideRatingViewModel
    {
        public int Rating { get; set; }

        public string Description { get; set; }

        public TravelerViewModel Traveler { get; set; }

        public DateTime? Created { get; set; }
    }

    public class GuideViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Tags { get; set; }

    }

}