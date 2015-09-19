using System;
using System.Threading.Tasks;
using OutsourceFriends.Context;
using OutsourceFriends.Models;
using System.Linq;

namespace OutsourceFriends.Helpers
{
    public class GuideHelper
    {
        public static async Task<Guide> CreateGuidIfNotExists(DomainManager manager, ApplicationUser user, string imageUrl)
        {
            if (user.Guide == null)
            {
                Guide g = new Guide()
                {
                    User = user,
                    UserId = user.Id,
                    Age = 0,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    LastActive = DateTime.UtcNow,
                    Removed = false,
                    ShowInSearch = false,
                    MinBudget = 0,
                };

                manager.db.Guides.Add(g);
                await manager.save();

                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    await ImageHelper.setImageFromUrl(manager, imageUrl, g);
                }

            }
            return user.Guide;
        }


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

        public static System.Linq.Expressions.Expression<Func<GuideRating, GuideRatingViewModel>> guideRatingViewSelector = (x) =>
            new GuideRatingViewModel()
            {
                Rating = x.Rating,
                Description = x.Description,
                Traveler = new TravelerViewModel
                {
                    ImageUrl = x.Traveler.ImageUrl,
                    Name = x.Traveler.Name,
                    Id = x.TravelerId
                },
                Created = x.Created
            };
    }
}