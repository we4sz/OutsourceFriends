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
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {

        [Route("Test")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Test()
        {
            return Ok(new { Login = true });
        }

        [Route("Populate")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Populate()
        {
            Random r = new Random();
            var users = await DomainManager.db.Users.Include(x => x.Guide).Include(x => x.Traveler).Include(x => x.Guide.Ratings).ToListAsync();
            
            foreach(ApplicationUser u in users)
            {
                Traveler t = u.Traveler;
                Guide g = u.Guide;

                t.Age = g.Age;
                t.Name = g.Name;
                t.Description = g.Description;
                t.ImageUrl = g.ImageUrl;
                t.Location = g.Location;

                DomainManager.updateEntity(t);
                await DomainManager.save();
            }

            foreach (ApplicationUser u in users)
            {
                Guide g = u.Guide;

                var results = await (from t in DomainManager.db.Travelers orderby Guid.NewGuid() select t).Take(r.Next(5,15)).ToListAsync();
                    
                foreach(Traveler t in results)
                {
                    g.Ratings.Add(new GuideRating()
                    {

                        Created = DateTime.UtcNow,
                        Description = t.Description,
                        LastEdited = DateTime.UtcNow,
                        Traveler = t,
                        Rating = r.Next(2, 5),
                        Guide = g,
                        GuideId = g.UserId,
                        TravelerId = t.UserId
                    });

                }

                try {
                    DomainManager.updateEntity(g);
                    await DomainManager.save();
                }catch(Exception)
                {

                }

            }




                return Ok();
        }



    }
}
