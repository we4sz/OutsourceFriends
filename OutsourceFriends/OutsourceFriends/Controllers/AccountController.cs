using SelfieJobs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OutsourceFriends.Controllers
{

    [Authorize]
    [RequireSecureConnectionFilter]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        
        [Route("Test")]
        [HttpGet]
        public IHttpActionResult Test()
        {
            return Ok(new { Login = true });
        }



    }
}
