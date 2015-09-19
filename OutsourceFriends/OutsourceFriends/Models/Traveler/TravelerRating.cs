using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class TravelerRating
    {
        public virtual Guide Guide { get; set; }

        [Required]
        [Key]
        [Column(Order = 1)]
        public string GuideId { get; set; }


        public virtual Traveler Traveler { get; set; }

        [Required]
        [Key]
        [Column(Order = 0)]
        public string TravelerId { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LastEdited { get; set; }
    }
}