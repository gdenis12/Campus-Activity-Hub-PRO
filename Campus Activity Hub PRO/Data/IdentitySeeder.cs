using Campus_Activity_Hub_PRO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Campus_Activity_Hub_PRO.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            await db.Database.MigrateAsync();


            string[] roles = { "Admin", "Organizer", "Student" };
            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                    await roleManager.CreateAsync(new IdentityRole(r));
            }


            var adminEmail = "admin@admin.com";
            var adminPassword = "Admin123!"; 

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Administrator",
                    EmailConfirmed = true
                };

                var createRes = await userManager.CreateAsync(admin, adminPassword);
                if (!createRes.Succeeded)
                    throw new Exception(string.Join("; ", createRes.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(admin, "Admin");
            }
            else
            {

                if (!await userManager.IsInRoleAsync(admin, "Admin"))
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}

