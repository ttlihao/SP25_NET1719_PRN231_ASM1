using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrowthTracker.Repositories.DBContext;
using GrowthTracker.Repositories.Models;
using Newtonsoft.Json;

namespace GrowthTracker.MVCWebApp.Controllers
{
    public class OrdersController : Controller
    {
        //    private readonly PRN321_DatabaseContext _context;
        private string APIEndPoint = "https://localhost:7066/api/";

        //    public OrdersController(PRN321_DatabaseContext context)
        //    {
        //        _context = context;
        //    }

        public OrdersController()
        {
        }

        //    // GET: Orders
        //    public async Task<IActionResult> Index()
        //    {
        //        var pRN321_DatabaseContext = _context.Orders.Include(o => o.Account);
        //        return View(await pRN321_DatabaseContext.ToListAsync());
        //    }
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                #region Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                #endregion

                using (var response = await httpClient.GetAsync(APIEndPoint + "Order"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<Order>>(content);

                        if (result != null)
                        {
                            return View(result);
                        }
                    }
                }
            }
            return View(new List<Order>());
        }
        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        //    // GET: Orders/Details/5
        //    public async Task<IActionResult> Details(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var order = await _context.Orders
        //            .Include(o => o.Account)
        //            .FirstOrDefaultAsync(m => m.OrderId == id);
        //        if (order == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(order);
        //    }

        //    // GET: Orders/Create
        //    public IActionResult Create()
        //    {
        //        ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Password");
        //        return View();
        //    }

        //    // POST: Orders/Create
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create([Bind("OrderId,AccountId,OrderDate")] Order order)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(order);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Password", order.AccountId);
        //        return View(order);
        //    }

        //    // GET: Orders/Edit/5
        //    public async Task<IActionResult> Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var order = await _context.Orders.FindAsync(id);
        //        if (order == null)
        //        {
        //            return NotFound();
        //        }
        //        ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Password", order.AccountId);
        //        return View(order);
        //    }

        //    // POST: Orders/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(int id, [Bind("OrderId,AccountId,OrderDate")] Order order)
        //    {
        //        if (id != order.OrderId)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(order);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!OrderExists(order.OrderId))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Password", order.AccountId);
        //        return View(order);
        //    }

        //    // GET: Orders/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var order = await _context.Orders
        //            .Include(o => o.Account)
        //            .FirstOrDefaultAsync(m => m.OrderId == id);
        //        if (order == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(order);
        //    }

        //    // POST: Orders/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        var order = await _context.Orders.FindAsync(id);
        //        if (order != null)
        //        {
        //            _context.Orders.Remove(order);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool OrderExists(int id)
        //    {
        //        return _context.Orders.Any(e => e.OrderId == id);
        //    }
        //}
    }
}
