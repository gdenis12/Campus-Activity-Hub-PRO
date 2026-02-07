using Campus_Activity_Hub_PRO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Campus_Activity_Hub_PRO.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();




            string[] roles = { "Admin", "Organizer", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }


            if (await userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                var admin = new AppUser
                {
                    UserName = "Lyoha",
                    Email = "admin@admin.com",
                    Name = "Administrator"
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            var students = new[]
{
                new AppUser
                {
                    UserName = "Igor",
                    Email = "student1@test.com",
                    Name = "Student One"
                },
                new AppUser
                {
                    UserName = "Bohdan",
                    Email = "student2@test.com",
                    Name = "Student Two"
                },
                new AppUser
                {
                    UserName = "Artem",
                    Email = "student3@test.com",
                    Name = "Student Three"
                },
                new AppUser
                {
                    UserName = "Nikita",
                    Email = "student4@test.com",
                    Name = "Student Four"
                },
                new AppUser
                {
                    UserName = "Denis",
                    Email = "student5@test.com",
                    Name = "Student Five"
                }
            };

            foreach (var student in students)
            {
                if (await userManager.FindByEmailAsync(student.Email) == null)
                {
                    await userManager.CreateAsync(student, "Student123!");
                    await userManager.AddToRoleAsync(student, "Student");
                }
            }


            var organizers = new[]
            {
                new AppUser
                {
                    UserName = "Vlad",
                    Email = "org1@test.com",
                    Name = "Vlad Volodymyrovych"
                },
                new AppUser
                {
                    UserName = "Ivan",
                    Email = "org2@test.com",
                    Name = "Ivan Vasylovich"
                }
            };

            foreach (var org in organizers)
            {
                if (await userManager.FindByEmailAsync(org.Email) == null)
                {
                    await userManager.CreateAsync(org, "Organizer123!");
                    await userManager.AddToRoleAsync(org, "Organizer");
                }
            }


            string[] categories = { "Sports", "Education", "Entertainment" };

            foreach (var name in categories)
            {
                if (!await context.Categories.AnyAsync(c => c.Name == name))
                {
                    context.Categories.Add(new Category { Name = name });
                }
            }

            await context.SaveChangesAsync();



            if (!await context.Events.AnyAsync())
            {
                var sports = await context.Categories.FirstAsync(c => c.Name == "Sports");
                var education = await context.Categories.FirstAsync(c => c.Name == "Education");
                var entertainment = await context.Categories.FirstAsync(c => c.Name == "Entertainment");

                var Vlad = await userManager.FindByEmailAsync("org1@test.com");
                var Ivan = await userManager.FindByEmailAsync("org2@test.com");

                if (sports == null || education == null || entertainment == null)
                    return;

                if (Vlad == null || Ivan == null)
                    return;

                context.Events.AddRange(
                    new Event
                    {
                        Title = "Football Tournament",
                        CategoryId = sports.Id,
                        OrganizerId = Vlad.Id,
                        EventDate = DateTime.Now.AddDays(5),
                        Capacity = 11,
                        PosterPath = "https://tse2.mm.bing.net/th/id/OIP.dpjw1juKqSX2kaA59LjzbQHaD4?rs=1&pid=ImgDetMain&o=7&rm=3"
                    },
                    new Event
                    {
                        Title = "Basketball Match",
                        CategoryId = sports.Id,
                        OrganizerId = Ivan.Id, 
                        EventDate = DateTime.Now.AddDays(7),
                        Capacity = 6,
                        PosterPath = "https://tse2.mm.bing.net/th/id/OIP.dkKF4a8UKWOEAJJlfAqyiQHaEo?rs=1&pid=ImgDetMain&o=7&rm=3"
                    },
                    new Event
                    {
                        Title = "Math Workshop",
                        CategoryId = education.Id,
                        OrganizerId = Vlad.Id,
                        EventDate = DateTime.Now.AddDays(9),
                        Capacity = 5,
                        PosterPath = "https://img.freepik.com/free-vector/maths-chalkboard_23-2148178220.jpg"
                    },
                    new Event
                    {
                        Title = "IT Meetup",
                        CategoryId = education.Id,
                        OrganizerId = Ivan.Id,
                        EventDate = DateTime.Now.AddDays(12),
                        Capacity = 40,
                        PosterPath = "https://tse4.mm.bing.net/th/id/OIP.8WmPvxo4FCORcX-G4m0GNQHaHa?rs=1&pid=ImgDetMain&o=7&rm=3"
                    },
                    new Event
                    {
                        Title = "Movie Night",
                        CategoryId = entertainment.Id,
                        OrganizerId = Vlad.Id,
                        EventDate = DateTime.Now.AddDays(15),
                        Capacity = 30,
                        PosterPath = "https://tse2.mm.bing.net/th/id/OIP.UAFl_3uFTKeHDOuk3Ts1fwHaE7?rs=1&pid=ImgDetMain&o=7&rm=3"
                    }
                );

                await context.SaveChangesAsync();
            }

        }
    }
}

