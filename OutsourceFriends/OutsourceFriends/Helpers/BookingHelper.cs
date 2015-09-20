using System;
using System.Threading.Tasks;
using OutsourceFriends.Context;
using OutsourceFriends.Models;
using System.Linq;

namespace OutsourceFriends.Helpers
{
    public class BookingHelper
    {



        public static System.Linq.Expressions.Expression<Func<Guide, GuideViewModel>> guideViewSelector = (x) =>
            new GuideViewModel()
            {
                Title = x.Title,
                Description = x.Description,
                Tags = x.Tags.Select(xx => xx.Name),
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Rating = x.Ratings.Any() ? x.Ratings.Average(xx => xx.Rating) : 0,
                Ratings = x.Ratings.Count(),
                Id = x.UserId
            };

    }
}