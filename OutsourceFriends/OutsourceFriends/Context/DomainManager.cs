using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using OutsourceFriends.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace OutsourceFriends.Context
{
    public class DomainManager : IDisposable
    {

        private ApplicationUserManager _userManager;
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


        public DomainContext db { get; set; }
        public DomainManager()
        {
            this.db = new DomainContext();
        }

        public static DomainManager Create()
        {
            return new DomainManager();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task save()
        {
            await db.SaveChangesAsync();
        }

        public void updateEntity(object user)
        {
            db.Entry(user).State = EntityState.Modified;
        }


        public async Task<IdentityResult> createUser(ApplicationUser user)
        {
            return await UserManager.CreateAsync(user);
        }

        public async Task<IdentityResult> createUser(ApplicationUser user, string password)
        {
            return await UserManager.CreateAsync(user, password);

        }


        public async Task setUserRoles(ApplicationUser user, ICollection<string> roles)
        {
            using (RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)))
            {
                await UserManager.RemoveFromRolesAsync(user.Id, (await getUserRoles(user)).ToArray());
                foreach (string role in roles.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!(await RoleManager.RoleExistsAsync(role)))
                    {
                        await RoleManager.CreateAsync(new IdentityRole() { Name = role });
                    }
                    await UserManager.AddToRoleAsync(user.Id, role);
                }
            }

        }


        public async Task addUserRoles(ApplicationUser user, ICollection<string> roles)
        {
            using (RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)))
            {
                foreach (string role in roles.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!(await RoleManager.RoleExistsAsync(role)))
                    {
                        await RoleManager.CreateAsync(new IdentityRole() { Name = role });
                    }
                    await UserManager.AddToRoleAsync(user.Id, role);
                }
            }

        }

        public async Task<IQueryable<ApplicationUser>> getUsersByRole(string role)
        {
            using (RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)))
            {
                IdentityRole irole = await RoleManager.FindByNameAsync(role);
                if (irole == null)
                {
                    return Enumerable.Empty<ApplicationUser>().AsQueryable();
                }
                await db.Entry(irole).Collection(x => x.Users).LoadAsync();
                ICollection<string> accounts = irole.Users.Select(x => x.UserId).ToList();
                return db.Users.Where(x => accounts.Contains(x.Id) && !x.Removed);
            }
        }


        public async Task<ApplicationUser> getUserByEmail(string email)
        {
            try
            {
                ApplicationUser u = await db.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

                if (u == null)
                {
                    email = email.Replace(" ", "+");
                    u = await db.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
                    if (u == null || u.Removed)
                    {
                        return null;
                    }
                }
                else if (u.Removed)
                {
                    return null;
                }

                return u;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<IdentityResult> addUserPassword(string userID, string newpassword)
        {

            return await UserManager.AddPasswordAsync(userID, newpassword);

        }


        public async Task<List<string>> getUserRoles(ApplicationUser applier)
        {

            return (await UserManager.GetRolesAsync(applier.Id)).ToList();

        }

        public async Task<List<string>> getUserRoles(string id)
        {

            return (await UserManager.GetRolesAsync(id)).ToList();

        }


        public async Task<IEnumerable<string>> findUserIds(double latitude, double longitude, SearchSort? sort,int? maxbudget, List<string> ids, int amount, int range)
        {

            List<string> list = null;
            try
            {
                list = await db.Database.SqlQuery<string>(buildFindGuideSql(amount,longitude,latitude,sort,maxbudget,ids,range)).ToListAsync();
            }
            catch
            {

            }
            return list;
        }



        private string buildFindGuideSql(int amount, double longitude, double latitude, SearchSort? Sort, int? maxbudget, List<string> ids, int range)
        {
            string sort = "NEWID()";
            string location = "geography::STPointFromText('POINT(" + longitude.ToString().Replace(",", ".") + " " + latitude.ToString().Replace(",", ".") + ")', 4326)";

            if (Sort.HasValue)
            {
                if (Sort == SearchSort.RATING)
                {
                    sort = "(select avg(Rating) from GuideRatings as gr where gr.Guideid = g.userid) desc";
                }
            }

            if (ids != null)
            {
                ids = ids.Select(x =>
                {
                    Match match = Regex.Match(x, @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}");
                    if (match.Success)
                    {
                        return match.Value;
                    }
                    return null;
                }).Where(x => x != null).ToList();
            }

            return
                "select top " + amount + " " +

                @"g.UserId 
		        from Guides as g with(nolock,Index(SPINDEX_Guide))
		        where "

                + (maxbudget > 0 ? " g.minbudget < " + maxbudget + " and " : "")

                + (ids != null && ids.Count > 0 ? " g.userid not in (" + string.Join(",", ids.Select(x => "'" + x + "'")) + ") and " : "")

                + " g.ShowInSearch = 1 and g.Removed = 0"
                + " and g.Location.STDistance(" + location + ") < "+range+@"*1000   
		        order by " + sort;
        }



    }


}