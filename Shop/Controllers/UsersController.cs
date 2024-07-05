using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/Admin/[controller]/{action=Index}/{id?}")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly int pageSize = 10;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index(int? pageIndex)
        {
            IQueryable<ApplicationUser> query = userManager.Users.OrderByDescending(u => u.CreatedAt);

            if (pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip(((int)pageIndex - 1) * pageSize).Take(pageSize);

            var users = query.ToList();

            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;


            return View(users);
        }


        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var appUser = await userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            ViewBag.Roles = await userManager.GetRolesAsync(appUser);

            // get available roles
            var availableRoles = roleManager.Roles.ToList();
            var items = new List<SelectListItem>();
            foreach (var role in availableRoles)
            {
                items.Add(new SelectListItem
                {
                    Text = role.NormalizedName,
                    Value = role.Name,
                    Selected = await userManager.IsInRoleAsync(appUser, role.Name!)
                });
            }

            ViewBag.SelectItems = items;
            return View(appUser);
        }

        public async Task<IActionResult> EditRole(string? id, string? newRole)
        {
            if (id == null || newRole == null)
            {
                TempData["ErrorMessage"] = "Invalid user ID or role.";
                return RedirectToAction("Index", "Users");
            }

            var roleExists = await roleManager.RoleExistsAsync(newRole);
            var appUser = await userManager.FindByIdAsync(id);

            if (appUser == null || !roleExists)
            {
                TempData["ErrorMessage"] = "User not found or role does not exist.";
                return RedirectToAction("Index", "Users");
            }

            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser!.Id == appUser.Id)
            {
                TempData["ErrorMessage"] = "You can't change your own role!";
                return RedirectToAction("Details", "Users", new { id });
            }

            // Check if the new role is the same as the current role
            var userRoles = await userManager.GetRolesAsync(appUser);
            if (userRoles.Contains(newRole))
            {
                TempData["ErrorMessage"] = "The user already has this role.";
                return RedirectToAction("Details", "Users", new { id });
            }

            // update role
            foreach (var role in userRoles)
            {
                await userManager.RemoveFromRoleAsync(appUser, role);
            }
            await userManager.AddToRoleAsync(appUser, newRole);

            TempData["SuccessMessage"] = "Role updated successfully!";
            return RedirectToAction("Details", "Users", new { id });
        }


        public async Task<IActionResult> DeleteAccount(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var appUser = await userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser!.Id == appUser.Id)
            {
                TempData["ErrorMessage"] = "You can't delete your own account!";
                return RedirectToAction("Details", "Users", new { id });
            }

            // delete user account
            var result = await userManager.DeleteAsync(appUser);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Account deleted successfully.";
                return RedirectToAction("Index", "Users");
                
            }

            TempData["ErrorMessage"] = "Unable to delete this account: " + result.Errors.FirstOrDefault()?.Description;
            return RedirectToAction("Details", "Users", new { id });
        }



    }
}
