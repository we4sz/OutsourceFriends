using Microsoft.AspNet.Identity.EntityFramework;
using OutsourceFriends.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Context
{
    public class DomainContext : IdentityDbContext<ApplicationUser>
    {

        public DomainContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            var objectContext = (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
            objectContext.SavingChanges += new EventHandler(context_SavingChanges);
        }


        // SavingChanges event handler. 
        private void context_SavingChanges(object sender, EventArgs e)
        {
            // Ensure that we are passed an ObjectContext
            ObjectContext context = sender as ObjectContext;
            if (context != null)
            {
                // Validate the state of each entity in the context 
                // before SaveChanges can succeed. 
                foreach (ObjectStateEntry entry in
                    context.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added | EntityState.Modified))
                {
                    if (!entry.IsRelationship && (entry.Entity is OnSavingListener))
                    {
                        var fullyloaded = false;

                        (entry.Entity as OnSavingListener).beforeSave(fullyloaded);
                    }
                }
            }
        }

        public static DomainContext Create()
        {
            return new DomainContext();
        }


        public DbSet<Traveler> Travelers { get; set; }


        public DbSet<Guide> Guides { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Traveler>().HasRequired(x => x.User).WithOptional(x => x.Traveler);
            modelBuilder.Entity<Guide>().HasRequired(x => x.User).WithOptional(x => x.Guide);



            base.OnModelCreating(modelBuilder);
        }
    }
}
