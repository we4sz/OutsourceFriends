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
    [RoutePrefix("api/Traveler")]
    public class TravelerController : BaseController
    {


        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            TravelerViewModel t = await DomainManager.db.Travelers.Select(TravelerHelper.travelerViewSelector).FirstOrDefaultAsync(x => x.Id == uid);
            if (t != null)
            {
                return Ok(t);
            }

            return NotFound();
        }

        [Route("{id}/rating")]
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


        [Route("{id}/rating")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRating(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();
            TravelerRating gt = await DomainManager.db.TravelerRating.FirstOrDefaultAsync(x => x.TravelerId == id && x.GuideId == uid);
            if (gt != null)
            {
                DomainManager.db.TravelerRating.Remove(gt);
                await DomainManager.save();
            }

            return BadRequest();
        }



        [Route("{id}/ratings")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetRatings(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            IEnumerable<TravelerRatingViewModel> ts = await DomainManager.db.TravelerRating.Where(x => x.TravelerId == id).Select(TravelerHelper.travelerRatingViewSelector).ToListAsync();
            return Ok(ts);
        }


        [Route("{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TravelerViewModel t = await DomainManager.db.Travelers.Select(TravelerHelper.travelerViewSelector).FirstOrDefaultAsync(x => x.Id == id);
            if (t != null)
            {
                return Ok(t);
            }

            return NotFound();
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            Traveler t = await DomainManager.db.Travelers.FirstOrDefaultAsync(x => x.UserId == uid);
            if (t != null)
            {
                DomainManager.db.Travelers.Remove(t);
                await DomainManager.save();
            }

            return NotFound();
        }

        [Route("Name")]
        [HttpPut]
        public async Task<IHttpActionResult> SetName(NameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            Traveler t = await DomainManager.db.Travelers.FirstOrDefaultAsync(x => x.UserId == uid);
            if (t != null)
            {
                t.Name = model.Name;
                DomainManager.updateEntity(t);
                await DomainManager.save();
            }

            return NotFound();
        }

        [Route("Location")]
        [HttpPut]
        public async Task<IHttpActionResult> SetLocation(LocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            Traveler t = await DomainManager.db.Travelers.FirstOrDefaultAsync(x => x.UserId == uid);
            if (t != null)
            {
                t.Location = DbGeography.FromText(string.Format("POINT({1} {0})", model.Latitude.ToString().Replace(",", "."), model.Longitude.ToString().Replace(",", ".")), 4326);
                DomainManager.updateEntity(t);
                await DomainManager.save();
            }

            return NotFound();
        }


        [Route("Description")]
        [HttpPut]
        public async Task<IHttpActionResult> SetDescription(DescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            Traveler t = await DomainManager.db.Travelers.FirstOrDefaultAsync(x => x.UserId == uid);
            if (t != null)
            {
                t.Description = model.Description;
                DomainManager.updateEntity(t);
                await DomainManager.save();
            }

            return NotFound();
        }
    }
}
