using OutsourceFriends.Context;
using OutsourceFriends.Models;
using SelfieJobs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using OutsourceFriends.Helpers;

namespace OutsourceFriends.Controllers
{
    [Authorize]
    [RequireSecureConnectionFilter]
    [RoutePrefix("api/Booking")]
    public class BookingController : BaseController
    {


        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddRating(string id, GuideRatingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();
            if (uid != id)
            {
                Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == uid);
                if (g != null)
                {
                    Traveler t = await DomainManager.db.Travelers.FirstOrDefaultAsync(x => x.UserId == id);
                    if (t != null)
                    {
                        t.Ratings.Add(new TravelerRating()
                        {
                            Created = DateTime.UtcNow,
                            Description = model.Description,
                            Traveler = t,
                            LastEdited = DateTime.UtcNow,
                            Rating = model.Rating,
                        });
                        DomainManager.updateEntity(t);
                        await DomainManager.save();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }


            return BadRequest();
        }


    }
}
