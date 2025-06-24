using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using AgriEnergy.Models;
using System.ComponentModel;


namespace AgriEnergy.Data
{
    public static class DbInitializer
    {

        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserApplication>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

           
            string[] roles = { "Farmer", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var farmerUser = new UserApplication { UserName = "Soyamapango@gmail.com", Email = "Soyamapango@gmail.com", Role = "Farmer", EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != farmerUser.UserName))
            {
                await userManager.CreateAsync(farmerUser, "P@ssword222!");
                await userManager.AddToRoleAsync(farmerUser, "Farmer");

                var farmer = new Farmer { Name = "Khanya Pango", Location = "KWT", UserId = farmerUser.Id };
                context.Farmers.Add(farmer);
                context.Products.AddRange(
                    new Product { Name = "Carrot", Category = "Vegetables", ProductionDate = DateTime.Now.AddDays(-10), Farmer = farmer },
                    new Product { Name = "Potatoes", Category = "Vegetables", ProductionDate = DateTime.Now.AddDays(-7), Farmer = farmer }
                );
            }

          
            var employeeUser = new UserApplication { UserName = "employee1@gmail.com", Email = "employee1@gmail.com", Role = "Employee", EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != employeeUser.UserName))
            {
                await userManager.CreateAsync(employeeUser, "P@ssword222!");
                await userManager.AddToRoleAsync(employeeUser, "Employee");
            }

            await context.SaveChangesAsync();
        }
    }
}
