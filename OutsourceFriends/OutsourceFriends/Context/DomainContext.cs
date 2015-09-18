using Microsoft.AspNet.Identity.EntityFramework;
using OutsourceFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutsourceFriends.Context
{
    public class DomainContext: IdentityDbContext<ApplicationUser>
    {

            public DomainContext()
                : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

            public static DomainContext Create()
            {
                return new DomainContext();
            }
        }
    }
}