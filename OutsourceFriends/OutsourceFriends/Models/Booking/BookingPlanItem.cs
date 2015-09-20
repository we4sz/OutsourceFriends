using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace OutsourceFriends.Models
{
    public class BookingPlanItem
    {

        public virtual BookingRequest Booking { get; set; }

        [Required]
        public Int64 BookingId { get; set; }



        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int64 Id { get; set; }

        public int Duration { get; set; }

        public int Amount { get; set; }

        public string Title { get; set; }


    }
}