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

        public BookingRequest()
        {
            Dates = new List<BookingDate>();
            PlanItems = new List<BookingPlanItem>();
        }

        public virtual Guide Guide { get; set; }

        [Required]
        public string GuideId { get; set; }

        public virtual Traveler Traveler { get; set; }

        [Required]
        public string TravelerId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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


        public virtual ICollection<BookingPlanItem> PlanItems { get; set; }

    }
}