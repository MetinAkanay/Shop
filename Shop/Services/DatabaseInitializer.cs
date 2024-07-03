using Microsoft.AspNetCore.Identity;
using Shop.Models;

namespace Shop.Services
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
            RoleManager<IdentityRole>? roleManager)
        {
            if(userManager == null || roleManager == null)
            {
                Console.WriteLine("userManager or roleManager is null => exit");
                return;
            }

            //check if we have the admin role 
            var exists = await roleManager.RoleExistsAsync("Admin");
            if (!exists)
            {
                Console.WriteLine("Admin role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            //check if we have the seller role 
            exists = await roleManager.RoleExistsAsync("Seller");
            if (!exists)
            {
                Console.WriteLine("Seller role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("Seller"));
            }

            //check if we have the client role 
            exists = await roleManager.RoleExistsAsync("Client");
            if (!exists)
            {
                Console.WriteLine("Client role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }

            //check if whe have at least one admin user or not
            var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
            if (adminUsers.Any())
            {
                Console.WriteLine("Admin user already exists => exit");
                return;
            }


            //create the admin user
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
                // set the user role
                await userManager.AddToRoleAsync(user, "Admin");
                Console.WriteLine("Admin user created succesfully! Please update the initial password");
                Console.WriteLine("Email: " + user.Email);
                Console.WriteLine("Initial password: " + initialPassword);
            }
        }
    }
}
