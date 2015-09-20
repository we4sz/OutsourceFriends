﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class BookingRequest
    {

        public BookingRequest()
        {
            Dates = new List<BookingDate>();
        }

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column(Order = 2)]
        public Int64 Id { get; set; }

        [Index]
        public int MinAmount { get; set; }

        [Index]
        public int MaxAmount { get; set; }

        public virtual ICollection<BookingDate> Dates { get; set; }

        [Index]
        public DateTime? AcceptedDate { get; set; }

        [Index]
        public DateTime? Created { get; set; }

        [Index]
        [MaxLength(256)]
        public string TransactionId { get; set; }


    }
}