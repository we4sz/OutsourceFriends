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
        public static async Task<Traveler> CreateTravelerIfNotExists(DomainManager manager, ApplicationUser user)
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
            }
            return user.Traveler;
        }


        public static System.Linq.Expressions.Expression<Func<Traveler, TravelerViewModel>> travelerViewSelector = (x) =>
        new TravelerViewModel()
        {
            Name = x.Name,
            ImageUrl = x.ImageUrl
        };
    }
}