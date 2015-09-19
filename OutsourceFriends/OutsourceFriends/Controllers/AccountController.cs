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
using OutsourceFriends.Helpers;

namespace SelfieJobs.Controllers.MVC
{
    [Authorize]
    [RequireSecureConnectionFilter]
    public class AccountController : BaseController
    {


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginFacebookViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false, Error = GetAllErros(ModelState) }); 
            }

            SignInStatusData data = await UserHelper.FacebookLogin(SelfiejobsManager, model.ProfileType, model.Token, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            if (data.Status == SignInStatus.Success)
            {
                await SignInManager.SignInAsync(data.SignInUser, true, true);
                return Json(new { Success = true });
            }
            else
            {
                return Json(new { Success = false, Errors = data.SignInError });
            }

        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                string cookieName = Request.Cookies[i].Name;
                var aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
            AuthenticationManager.SignOut();
            return Json(new { Success = true });
        }







    }
}