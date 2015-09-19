using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OutsourceFriends.Context;
using OutsourceFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;
using SelfieJobs.Controllers;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Web;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace OutsourceFriends.Helpers
{

    public class SignInStatusData
    {
        public SignInStatusData()
        {
            Confirmed = true;
            Status = SignInStatus.Failure;
        }

        public SignInStatus Status { get; set; }

        public string SignInError { get; set; }

        public ApplicationUser SignInUser { get; set; }

        public bool Confirmed { get; set; }


    }

    public class UserHelper
    {


        public static async Task<SignInStatusData> Login(DomainManager manager, ApplicationSignInManager SignInManager, string email, string password, bool remember, ProfileType profileType, string language)
        {
            SignInStatusData data = new SignInStatusData();
            if (string.IsNullOrWhiteSpace(email))
            {
                data.SignInError = "101";
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                data.SignInError = "104";
            }
            else
            {
                ApplicationUser user = await manager.getUserByEmail(email);
                if (user == null)
                {
                    data.SignInError = "308";
                }
                else if (!user.EmailConfirmed)
                {
                    data.Confirmed = false;
                    data.SignInError = "307";
                }
                else
                {
                    data.Status = await SignInManager.PasswordSignInAsync(user.Email, password, remember, shouldLockout: true);
                    if (data.Status == SignInStatus.Success)
                    {
                        data.SignInUser = user;
                        if (profileType == ProfileType.TRAVELER)
                        {
                            await TravelerHelper.CreateTravelerIfNotExists(manager, user);
                        }
                        else
                        {
                            await GuideHelper.CreateGuidIfNotExists(manager, user);
                        }
                    }
                    else if (data.Status == SignInStatus.LockedOut)
                    {
                        data.SignInError = "305";
                    }
                    else if (data.Status == SignInStatus.RequiresVerification)
                    {
                        data.SignInError = "301";
                    }
                    else
                    {
                        data.SignInError = "308";
                    }
                }
            }

            if (data.Status == SignInStatus.Success && data.SignInUser != null && string.IsNullOrWhiteSpace(data.SignInUser.UserLanguage))
            {
                data.SignInUser.UserLanguage = language ?? "sv";
                manager.updateEntity(data.SignInUser);
                await manager.save();
            }

            return data;
        }


        public static async Task<SignInStatusData> FacebookLogin(DomainManager manager, ProfileType profileType, string token, string language)
        {
            SignInStatusData data = new SignInStatusData();

            try
            {
                FacebookClient client = new FacebookClient(token);
                dynamic me = client.Get("me");
                string mail = me.email;
                string id = me.id;
                if (mail == null)
                {
                    data.SignInError = "200";
                }
                else if (id == null)
                {
                    data.SignInError = "204";
                }
                else
                {
                    ICollection<ApplicationUser> users = await (await manager.getUsersByRole("facebookid" + id)).Include(x => x.Guide).Include(x => x.Traveler).ToListAsync();
                    if (users == null || users.Count > 1)
                    {
                        data.SignInError = "203";
                    }
                    else if (users.Count == 0)
                    {
                        ApplicationUser emailUser = await manager.getUserByEmail(mail);
                        if (emailUser != null)
                        {
                            data.SignInError = "202";
                        }
                        else
                        {
                            return await CreateUser(manager, profileType, mail, null, id, language);
                        }
                    }
                    else
                    {
                        var user = users.First();
                        data.SignInUser = user;
                        if (profileType == ProfileType.TRAVELER)
                        {
                            await TravelerHelper.CreateTravelerIfNotExists(manager, user);
                        }
                        else
                        {
                            await GuideHelper.CreateGuidIfNotExists(manager, user);
                        }
                        data.Status = SignInStatus.Success;
                    }
                }
            }
            catch (Exception)
            {
                data.SignInError = "204";
            }
            return data;
        }


        public static async Task<string> AddFacebookToLogin(DomainManager manager, ApplicationUser user, string token)
        {
            FacebookClient client = new FacebookClient(token);
            dynamic me = client.Get("me");
            string id = me.id;

            if (id == null)
            {
                return "200";
            }
            ICollection<ApplicationUser> users = await (await manager.getUsersByRole("facebookid" + id)).ToListAsync();
            if (users.Count > 0)
            {
                return "201";
            }
            await manager.addUserRoles(user, new List<string>() { "facebook", "facebookid" + id });
            return null;
        }

        public static async Task<string> RemoveFacebookFromLogin(DomainManager manager, ApplicationUser user)
        {
            if (user.PasswordHash == null)
            {
                return "306";
            }

            ICollection<string> roles = await manager.getUserRoles(user);
            await manager.setUserRoles(user, roles.Where(x => !x.ToLower().Contains("facebook")).ToList());
            return null;
        }


        public static async Task<SignInStatusData> CreateUser(DomainManager manager, ProfileType profileType, string mail, string password, string facebookid, string language)
        {
            SignInStatusData data = new SignInStatusData();

            ApplicationUser user = new ApplicationUser()
            {
                CreatedTime = DateTime.UtcNow,
                UserName = mail,
                Email = mail,
                EmailConfirmed = true,
                LastActive = DateTime.UtcNow,
                UserLanguage = language ?? "sv",
                Emails = true,
            };

            if (password == null)
            {
                IdentityResult result = await manager.createUser(user);
                if (!result.Succeeded)
                {
                    data.SignInError = string.Join(", ", result.Errors);
                    return data;
                }
            }
            else
            {
                IdentityResult result = await manager.createUser(user, password);
                if (!result.Succeeded)
                {
                    data.SignInError = string.Join(", ", result.Errors);
                    return data;
                }
            }

            if (facebookid != null)
            {
                await manager.addUserRoles(user, new List<string>() { "facebook", "facebookid" + facebookid });
            }



            if (profileType == ProfileType.TRAVELER)
            {
                Traveler app = await TravelerHelper.CreateTravelerIfNotExists(manager, user);
                await manager.save();
                //Task.Run(() => MailHandler.sendPrivateWelcomeMail(app, url));
            }
            else
            {
                await GuideHelper.CreateGuidIfNotExists(manager, user);
                await manager.save();
                //Task.Run(() => MailHandler.sendOrganisationWelcomeMail(user, url));
            }

            data.Status = SignInStatus.Success;
            data.SignInUser = user;
            return data;
        }





    }
}