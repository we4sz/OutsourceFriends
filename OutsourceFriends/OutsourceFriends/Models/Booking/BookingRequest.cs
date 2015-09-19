using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class BookingRequest
    {

        public virtual Guide Guide { get; set; }

        [Required]
        [Key]
        [Column(Order = 0)]
        public string GuideId { get; set; }

        public virtual Traveler Traveler { get; set; }

        [Required]
        [Key]
        [Column(Order = 1)]
        public string TravelerId { get; set; }

        [Required]
        [Key]
        [Column(Order = 2)]
        public DateTime? Date { get; set; }

        public int Amount { get; set; }

        [Index]
        public bool? Accepted { get; set; }

        [Index]
        public DateTime? Created { get; set; }



    }
}