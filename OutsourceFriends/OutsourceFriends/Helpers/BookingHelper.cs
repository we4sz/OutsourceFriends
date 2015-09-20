using System;
using System.Threading.Tasks;
using OutsourceFriends.Context;
using OutsourceFriends.Models;
using System.Linq;

namespace OutsourceFriends.Helpers
{
    public class BookingHelper
    {

        public static Func<BookingRequest, BookingRequstViewModel> bookingViewFunc = (ls) =>
    new BookingRequstViewModel()
    {
        Id = ls.Id,
        Traveler = new TravelerViewModel
        {
            Id = ls.TravelerId,
            ImageUrl = ls.Traveler.ImageUrl,
            Name = ls.Traveler.Name
        },
        Plans = ls.PlanItems.Select(x => new BookingPlanItemViewModel()
        {
            Title = x.Title,
            Amount = x.Amount,
            Duration = x.Duration,
            Id = x.Id
        }),
        Dates = ls.Dates.Select(x => new BookingDateViewModel() { Date = x.Date.Value, Id = x.Id })
    };

        public static System.Linq.Expressions.Expression<Func<BookingRequest, BookingRequstViewModel>> bookingViewSelector = (ls) =>
            new BookingRequstViewModel()
            {
                Id = ls.Id,
                Traveler = new TravelerViewModel
                {
                    Id = ls.TravelerId,
                    ImageUrl = ls.Traveler.ImageUrl,
                    Name = ls.Traveler.Name
                },
                Plans = ls.PlanItems.Select(x => new BookingPlanItemViewModel()
                {
                    Title = x.Title,
                    Amount = x.Amount,
                    Duration = x.Duration,
                    Id = x.Id
                }),
                Dates = ls.Dates.Select(x => new BookingDateViewModel() { Date = x.Date.Value, Id = x.Id })
            };

    }
}