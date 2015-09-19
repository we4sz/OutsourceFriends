using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using OutsourceFriends.Context;
using OutsourceFriends.Models;

namespace OutsourceFriends.Helpers
{
    public class TravelerHelper
    {
        public static async Task<Traveler> CreateTravelerIfNotExists(DomainManager manager, ApplicationUser user, string imageUrl)
        {
            if (user.Traveler == null)
            {
                Traveler t = new Traveler()
                {
                    User = user,
                    UserId = user.Id,
                    Age = 0,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    LastActive = DateTime.UtcNow,
                    Removed = false
                };

                manager.db.Travelers.Add(t);
                await manager.save();

                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    await ImageHelper.setImageFromUrl(manager, imageUrl, t);
                }
            }
            return user.Traveler;
        }


        public static System.Linq.Expressions.Expression<Func<Traveler, TravelerViewModel>> travelerViewSelector = (x) =>
        new TravelerViewModel()
        {
            Name = x.Name,
            Id = x.UserId,
            ImageUrl = x.ImageUrl,
            Rating = x.Ratings.Any() ? x.Ratings.Average(xx => xx.Rating) : 0,
            Ratings = x.Ratings.Count()
        };

        public static System.Linq.Expressions.Expression<Func<TravelerRating, TravelerRatingViewModel>> travelerRatingViewSelector = (x) =>
        new TravelerRatingViewModel()
        {
            Rating = x.Rating,
            Description = x.Description,
            Guide = new SmallGuideViewModel
            {
                ImageUrl = x.Guide.ImageUrl,
                Name = x.Guide.Name,
                Title = x.Guide.Title,
                Id = x.Guide.UserId
            },
            Created = x.Created
        };
    }
}