using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class PaymentViewModel
    {
        public string Nounce { get; set; }

        public Int64 BookingId { get; set; }

        public string GuideId { get; set; }

    }
}