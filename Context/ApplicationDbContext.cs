using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Módulo_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Módulo_7.Context
{

                                     // Microsoft.AspNetCore.Identity.EntityFrameworkCore; | version 2.2.0, because error, so wrong, so bad.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {                                // Contexto de datos configurado para trabajar con las tablas básicas de un sistema de login  

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            var roleAdmin = new IdentityRole()
            {
                Id = "5222c66e-55de-4dcf-b597-d921c2fe64bd",
                Name = "admin",
                NormalizedName = "admin"
            };

            builder.Entity<IdentityRole>().HasData(roleAdmin);

            base.OnModelCreating(builder);
        }

    }
}
