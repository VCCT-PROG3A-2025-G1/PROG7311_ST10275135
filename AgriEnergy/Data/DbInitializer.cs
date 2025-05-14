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

            // Seed roles
            string[] roles = { "Farmer", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed a Farmer user
            var farmerUser = new UserApplication { UserName = "farmer1@demo.com", Email = "farmer1@demo.com", Role = "Farmer", EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != farmerUser.UserName))
            {
                await userManager.CreateAsync(farmerUser, "Password123!");
                await userManager.AddToRoleAsync(farmerUser, "Farmer");

                var farmer = new Farmer { Name = "John Doe", Location = "Limpopo", UserId = farmerUser.Id };
                context.Farmers.Add(farmer);
                context.Products.AddRange(
                    new Product { Name = "Tomatoes", Category = "Vegetables", ProductionDate = DateTime.Now.AddDays(-10), Farmer = farmer },
                    new Product { Name = "Spinach", Category = "Vegetables", ProductionDate = DateTime.Now.AddDays(-7), Farmer = farmer }
                );
            }

            // Seed an Employee user
            var employeeUser = new UserApplication { UserName = "employee1@demo.com", Email = "employee1@demo.com", Role = "Employee", EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != employeeUser.UserName))
            {
                await userManager.CreateAsync(employeeUser, "Password123!");
                await userManager.AddToRoleAsync(employeeUser, "Employee");
            }

            await context.SaveChangesAsync();
        }
    }
}
