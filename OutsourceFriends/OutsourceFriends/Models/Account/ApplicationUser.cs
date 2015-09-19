using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OutsourceFriends.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        [Index]
        [MaxLength(2)]
        public string UserLanguage { get; set; }
        
        [DefaultValue(true)]
        public bool Emails { get; set; }

        public virtual Traveler Traveler { get; set; }

        public virtual Guide Guide { get; set; }

        [Index]
        [DefaultValue(false)]
        public bool Removed { get; set; }

        public override string UserName { get; set; }

        public ApplicationUser()
            : base()
        {

        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }



        public DateTime? CreatedTime { get; set; }

        public DateTime? LastActive { get; set; }
    }


}