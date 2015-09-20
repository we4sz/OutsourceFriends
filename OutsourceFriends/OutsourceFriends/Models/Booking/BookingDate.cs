using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class BookingDate
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


        public virtual BookingRequest Booking { get; set; }

        [Required]
        [Key]
        [Column(Order = 2)]
        public Int64 BookingId { get; set; }

        

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column(Order = 3)]
        public Int64 Id { get; set; }


        [Index]
        public DateTime? Date { get; set; }

    }
}