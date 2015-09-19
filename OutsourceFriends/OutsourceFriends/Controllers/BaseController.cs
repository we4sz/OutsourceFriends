using Microsoft.AspNet.Identity.Owin;
using System.Web.Http;
using System.Net.Http;
using System.Web;
using OutsourceFriends.Context;
using SelfieJobs.Filters;

namespace OutsourceFriends.Controllers
{
    [Authorize]
    [RequireSecureConnectionFilter]
    public class BaseController : ApiController
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private DomainManager _domainManager;


        public DomainManager DomainManager
        {
            get
            {
                return _domainManager ?? HttpContext.Current.GetOwinContext().Get<DomainManager>();
            }
            private set
            {
                _domainManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }
}