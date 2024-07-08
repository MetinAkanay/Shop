﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Authorize(Roles = "Client")]
    [Route("/Client/Orders/{action=Index}/{id?}")]
    public class ClientOrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly int pageSize = 5;

        public ClientOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(int pageIndex)
        {

            var currentUser = await userManager.GetUserAsync(User);
            if(currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<Order> query = context.Orders.Include(o => o.Items).OrderByDescending(o => o.Id).Where(o => o.ClientId == currentUser.Id);

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var orders = query.ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;
            ViewBag.Orders = orders;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if(currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            var order = context.Orders.Include(o => o.Items).ThenInclude(oi => oi.Product).Where(o => o.ClientId == currentUser.Id).FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return RedirectToAction("Index");
            }

            return View(order);
        }
    }
}
