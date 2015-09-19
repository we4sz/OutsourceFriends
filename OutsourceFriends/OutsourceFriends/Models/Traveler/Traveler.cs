using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class Traveler
    {
        [Index(IsUnique = true)]
        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }


        public virtual ICollection<TravelerRating> Ratings { get; set; }

        public virtual ICollection<GuideRating> GivenRatings { get; set; }

        public virtual ICollection<BookingRequest> Bookings { get; set; }

        [Index]
        [MaxLength(256)]
        public string Name { get; set; }

        [Index]
        [DefaultValue(false)]
        public bool Removed { get; set; }

        [Index]
        public DateTime? UpdatedTime { get; set; }

        [Index]
        public DateTime? LastActive { get; set; }

        [Index]
        public int Age { get; set; }

        [Index]
        [MaxLength(256)]
        public string ImageUrl { get; set; }

        [Required]
        public DateTime? CreatedTime { get; set; }

        public DbGeography Location { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [MaxLength(2)]
        public string CreatedInLanguage { get; set; }

        public Traveler()
        {
            Ratings = new List<TravelerRating>();
            GivenRatings = new List<GuideRating>();
            Bookings = new List<BookingRequest>();
        }

        public void beforeSave(bool fullyLoaded)
        {
            UpdatedTime = DateTime.UtcNow;
        }

    }
}