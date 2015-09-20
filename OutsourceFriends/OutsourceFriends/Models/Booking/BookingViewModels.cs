using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Models
{
    public class BookingViewModel
    {
        public List<Int64> Dates { get; set; }

        public string GuideId { get; set; }

        public int MinAmount { get; set; }

        public int MaxAmount { get; set; }

    }
}