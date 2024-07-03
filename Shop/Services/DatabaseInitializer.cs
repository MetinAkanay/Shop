using Microsoft.AspNetCore.Identity;
using Shop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
            RoleManager<IdentityRole>? roleManager)
        {
            if (userManager == null || roleManager == null)
            {
                Console.WriteLine("UserManager or RoleManager is null => exit");
                return;
            }

            // Check if we have the admin role
            var exists = await roleManager.RoleExistsAsync("Admin");
            if (!exists)
            {
                Console.WriteLine("Admin role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Check if we have the seller role
            exists = await roleManager.RoleExistsAsync("Seller");
            if (!exists)
            {
                Console.WriteLine("Seller role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("Seller"));
            }

            // Check if we have the client role
            exists = await roleManager.RoleExistsAsync("Client");
            if (!exists)
            {
                Console.WriteLine("Client role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }

            // Check if we have at least one admin user
            var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
            if (adminUsers.Any())
            {
                Console.WriteLine("Admin user already exists => exit");
                return;
            }

            // Create the admin user
            var user = new ApplicationUser()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                CreatedAt = DateTime.Now
            };

            string initialPassword = "admin123";

            var result = await userManager.CreateAsync(user, initialPassword);
            if (result.Succeeded)
            {
                // Set the user role
                await userManager.AddToRoleAsync(user, "Admin");
                Console.WriteLine("Admin user created successfully! Please update the initial password.");
                Console.WriteLine("Email: " + user.Email);
                Console.WriteLine("Initial password: " + initialPassword);
            }
        }
    }
}