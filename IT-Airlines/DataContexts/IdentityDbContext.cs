using IT_Airlines.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_Airlines.DataContexts
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }
    }
}