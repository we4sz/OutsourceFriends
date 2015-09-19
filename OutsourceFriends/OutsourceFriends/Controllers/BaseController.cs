using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Globalization;
using System.Net.Http;
using System.IO;
using System.Drawing;
using SelfieJobs.Filters;
using OutsourceFriends.Controllers;
using OutsourceFriends.Models;
using OutsourceFriends.Context;

namespace OutsourceFriends.Controllers
{
    public class BaseController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;



        private DomainManager _domainManager;

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public DomainManager SelfiejobsManager
        {
            get
            {
                return _domainManager ?? HttpContext.GetOwinContext().Get<DomainManager>();
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
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }


        public void AddErrors(ModelStateDictionary ms, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ms.AddModelError("", error);
            }
        }

        public class JsonErrorModel
        {
            public List<string> Errors { get; set; }
        }

        public JsonErrorModel GetAllErros(ModelStateDictionary ms)
        {
            List<string> errors = new List<string>();
            foreach (ModelState modelState in ms.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return new JsonErrorModel() { Errors = errors };
        }

        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}