using OutsourceFriends.Models;
using OutsourceFriends.Context;
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
using System.Data.Entity.SqlServer;
using YelpSharp;
using YelpSharp.Data.Options;


namespace OutsourceFriends.Controllers
{
    [Authorize]
    [RequireSecureConnectionFilter]
    [RoutePrefix("api/Search")]
    public class SearchController : BaseController
    {



        Options options = new Options()
        {
            AccessToken = "qe8RGEFlDKVd7XSvD2yWrZIGdxLs8IWd",
            AccessTokenSecret = "_92b6L3jT7Le_HAD9xiuxHze5qM",
            ConsumerKey = "g-jEkqAHrCBsRjQT_SDoLg",
            ConsumerSecret = "t_QJVIQO9n1klQrOTQiCPXcPM7g"
        };



        [HttpPost]
        public async Task<IHttpActionResult> PostSearch(SearchViewModel model)
        {
            Yelp y = new Yelp(options);
            var result = await y.Search(model.Query, model.City);
            return Ok(result == null || result.businesses == null ? null : result.businesses.Select(x => new
            {
                Name = x.name,
                Id = x.id,
                Address = x.location.address,
                City = x.location.city,
                Lng = x.location.coordinate.longitude,
                Lat = x.location.coordinate.latitude,
                ImageUrl = x.image_url,
                Url = x.url,
                Phone = x.display_phone,
                Categories = x.categories.Select(xx => xx[0]),
                Rating = x.rating
            }));
        }

    }
}
