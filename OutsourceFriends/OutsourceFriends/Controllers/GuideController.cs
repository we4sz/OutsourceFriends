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
    [RoutePrefix("api/Guide")]
    public class GuideController : BaseController
    {

        
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            GuideViewModel g = await DomainManager.db.Guides.Select(GuideHelper.guideViewSelector).FirstOrDefaultAsync(x => x.Id == uid);
            if (g != null)
            {
                return Ok(g);
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
                Traveler t = await DomainManager.db.Travelers.FirstOrDefaultAsync(x => x.UserId == uid);
                if (t != null)
                {
                    Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == id);
                    if (g != null)
                    {
                        g.Ratings.Add(new GuideRating()
                        {
                            Created = DateTime.UtcNow,
                            Description = model.Description,
                            Traveler = t,
                            LastEdited = DateTime.UtcNow,
                            Rating = model.Rating,
                        });
                        DomainManager.updateEntity(g);
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
            GuideRating gt = await DomainManager.db.GuideRatings.FirstOrDefaultAsync(x => x.TravelerId == uid && x.GuideId == id);
            if(gt != null)
            {
                DomainManager.db.GuideRatings.Remove(gt);
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

            IEnumerable<GuideRatingViewModel> gs = await DomainManager.db.GuideRatings.Where(x => x.GuideId == id).Select(GuideHelper.guideRatingViewSelector).ToListAsync();
            return Ok(gs);
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
            GuideViewModel g = await DomainManager.db.Guides.Select(GuideHelper.guideViewSelector).FirstOrDefaultAsync(x => x.Id == id);
            if (g != null)
            {
                return Ok(g);
            }

            return NotFound();
        }

        [Route("Result")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetResult(GuideSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<string> ids = await DomainManager.findUserIds(model.Latitude, model.Longitude, model.Sort, model.MaxBudget, model.ids,10,50);
            IEnumerable<GuideViewModel> gs = await DomainManager.db.Guides.Where(x => ids.Any(xx => xx == x.UserId)).Select(GuideHelper.guideViewSelector).ToListAsync();
            return Ok(gs);
        }

        
        [HttpDelete]
        public async Task<IHttpActionResult> Delete()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                DomainManager.db.Guides.Remove(g);
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

            Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                g.Name = model.Name;
                DomainManager.updateEntity(g);
                await DomainManager.save();
            }

            return NotFound();
        }


        [Route("MinBudget")]
        [HttpPut]
        public async Task<IHttpActionResult> SetMinBudget(BudgetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                g.MinBudget = model.MinBudget;
                DomainManager.updateEntity(g);
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

            Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                g.Location = DbGeography.FromText(string.Format("POINT({1} {0})", model.Latitude.ToString().Replace(",", "."), model.Longitude.ToString().Replace(",", ".")), 4326);
                DomainManager.updateEntity(g);
                await DomainManager.save();
            }

            return NotFound();
        }

        [Route("Title")]
        [HttpPut]
        public async Task<IHttpActionResult> SetTitle(TitleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                g.Title = model.Title;
                DomainManager.updateEntity(g);
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

            Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                g.Description = model.Description;
                DomainManager.updateEntity(g);
                await DomainManager.save();
            }

            return NotFound();
        }

        [Route("Tag/{tag}")]
        [HttpPut]
        public async Task<IHttpActionResult> AddTag(string tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            string uid = User.Identity.GetUserId();

            Guide g = await DomainManager.db.Guides.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                if (g.Tags.Count < 3)
                {
                    g.Tags.Add(new Tag()
                    {
                        Name = tag
                    });
                }

                DomainManager.updateEntity(g);
                await DomainManager.save();
            }

            return NotFound();
        }

        [Route("Tag/{tag}")]
        [HttpDelete]
        public async Task<IHttpActionResult> RemoveTag(string tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            Guide g = await DomainManager.db.Guides.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UserId == uid);
            if (g != null)
            {
                g.Tags = g.Tags.Where(x => x.Name.Equals(tag, StringComparison.InvariantCultureIgnoreCase)).ToList();
                DomainManager.updateEntity(g);
                await DomainManager.save();
            }

            return NotFound();
        }

    }
}
