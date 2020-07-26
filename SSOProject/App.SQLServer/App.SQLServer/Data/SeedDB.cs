using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.SQLServer.Data
{
    public class SeedDB
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.EnsureCreated();

            if (!context.FieldTypes.Any())
            {
                var fieldType = new FieldType()
                {
                    ID = Guid.NewGuid(),
                    Name = "Text",
                };


                context.Entry<FieldType>(fieldType).State = EntityState.Detached;

                await context.FieldTypes.AddAsync(fieldType);

                var fieldType1 = new FieldType()
                {
                    ID = Guid.NewGuid(),
                    Name = "Date",
                };


                context.Entry<FieldType>(fieldType1).State = EntityState.Detached;

                await context.FieldTypes.AddAsync(fieldType1);

                var fieldType2 = new FieldType()
                {
                    ID = Guid.NewGuid(),
                    Name = "UserLoopUp",
                };


                context.Entry<FieldType>(fieldType2).State = EntityState.Detached;

                await context.FieldTypes.AddAsync(fieldType2);

                var fieldType3 = new FieldType()
                {
                    ID = Guid.NewGuid(),
                    Name = "Drop Down",
                };

                context.Entry<FieldType>(fieldType3).State = EntityState.Detached;

                await context.FieldTypes.AddAsync(fieldType3);

                await context.SaveChangesAsync();
            }

            if (!context.Tenants.Any())
            {
                var ten = new Tenant()
                {
                    Id = new Guid("7D7B4614-C733-4A6D-A09D-608B4351B852"),
                    Code = "DEI",
                    Name = "Date Experts, Inc.",
                    Email = "deiTenant@example.com",
                    };

                context.Entry<Tenant>(ten).State = EntityState.Detached;
                await context.Tenants.AddAsync(ten);

                var ten1 = new Tenant()
                {
                    Id = new Guid("7D7B4614-C733-4A6D-A09D-608B4351B827"),
                    Code = "ABCO",
                    Name = "ABC Co.",
                    Email = "abcTenant@example.com",
                };

                context.Entry<Tenant>(ten1).State = EntityState.Detached;
                await context.Tenants.AddAsync(ten1);
                await context.SaveChangesAsync();
            }

            if (!context.Roles.Any())
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                await roleManager.CreateAsync(role);
            }

            if (!context.Users.Any())
            {
                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@test.com";
                user.TenantCode = "ABCO";
                string userPWD = "Admin123!";
                await userManager.CreateAsync(user, userPWD);
                await userManager.AddToRoleAsync(user, "Admin");

            }

        }

    }
}
