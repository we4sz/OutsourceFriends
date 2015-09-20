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

    public class BookingDateViewModel
    {
        public Int64 Id { get; set; }

        public DateTime Date { get; set; }

    }

    public class BookingPlanViewModel
    {
        public string Title { get; set; }

        public int Duration { get; set; }

        public int Amount { get; set; }
    }


    public class BookingPlanItemViewModel
    {
        public Int64 Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public int Amount { get; set; }
    }


    public class BookingRequstViewModel
    {
        public IEnumerable<BookingDateViewModel> Dates { get; set; }

        public TravelerViewModel Traveler { get; set; }

        public int MinAmount { get; set; }

        public int MaxAmount { get; set; }

        public IEnumerable<BookingPlanItemViewModel> Plans { get; set; }

        public Int64 Id { get; set; }
    }



}