using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/Admin/Orders/{action=Index}/{id?}")]
    public class AdminOrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly int pageSize = 10;

        public AdminOrdersController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int pageIndex)
        {
            IQueryable<Order> query = context.Orders.Include(o => o.Client).Include(o => o.Items).OrderByDescending(o => o.Id);

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

        public IActionResult Details(int id)
        {
            var order = context.Orders.Include(o => o.Client).Include(o => o.Items).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.Id == id);

            if(order == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.NumOrders = context.Orders.Where(o => o.ClientId == order.ClientId).Count();

            return View(order);
        }   

        public IActionResult Edit(int id, string? payment_status, string? order_status)
        {

            var order = context.Orders.Find(id);
            if(order == null)
            {
                return RedirectToAction("Index");   
            }

            if(payment_status == null && order_status == null)
            {
                return RedirectToAction("Details", new { id });
            }

            if(payment_status != null)
            {
                order.PaymentStatus = payment_status;
            }

            if(order_status != null)
            {
                   order.OrderStatus = order_status;
            }

            context.SaveChanges();

            return RedirectToAction("Details", new { id });
        }
    }
}
