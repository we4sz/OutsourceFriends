using System;
using System.Threading.Tasks;
using OutsourceFriends.Context;
using OutsourceFriends.Models;

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
    }
}