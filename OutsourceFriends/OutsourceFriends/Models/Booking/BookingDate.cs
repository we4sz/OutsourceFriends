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

 

        public virtual BookingRequest Booking { get; set; }

        [Required]
        public Int64 BookingId { get; set; }

        

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int64 Id { get; set; }


        [Index]
        public DateTime? Date { get; set; }

    }
}