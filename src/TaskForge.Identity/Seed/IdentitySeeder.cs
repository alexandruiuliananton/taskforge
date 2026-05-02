using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskForge.Identity.Seed
{
    public class IdentitySeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            // 🔥 ROLE SEEDING
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                string[] roles = new[] { "User", "Admin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    }
                }
            }
        }
    }
}
