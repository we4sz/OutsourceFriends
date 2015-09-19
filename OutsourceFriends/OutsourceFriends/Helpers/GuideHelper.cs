using System;
using System.Threading.Tasks;
using OutsourceFriends.Context;
using OutsourceFriends.Models;
using System.Linq;

namespace OutsourceFriends.Helpers
{
    public class GuideHelper
    {
        public static async Task<Guide> CreateGuidIfNotExists(DomainManager manager, ApplicationUser user)
        {
            if (user.Traveler == null)
            {
                Guide g = new Guide()
                {
                    User = user,
                    UserId = user.Id,
                    Age = 0,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    LastActive = DateTime.UtcNow,
                    Removed = false
                };

                manager.db.Guides.Add(g);
                await manager.save();
            }
            return user.Guide;
        }


        public static System.Linq.Expressions.Expression<Func<Guide, GuideViewModel>> guideViewSelector = (x) =>
            new GuideViewModel()
            {
                Title = x.Title,
                Description = x.Description,
                Tags = x.Tags.Select(xx => xx.Name)
            };

        public static System.Linq.Expressions.Expression<Func<GuideRating, GuideRatingViewModel>> guideRatingViewSelector = (x) =>
            new GuideRatingViewModel()
            {
                Rating = x.Rating,
                Description = x.Description,
                Traveler = new TravelerViewModel
                {
                    ImageUrl = x.Traveler.ImageUrl,
                    Name = x.Traveler.Name
                },
                Created = x.Created
            };
    }
}