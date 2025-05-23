﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrowthTracker.Repositories.DBContext;
using GrowthTracker.Repositories.Models;
using Newtonsoft.Json;
using System.Text;

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
        public async Task<IActionResult> Create()
        {
            List<Account> accounts = new List<Account>();

            using (var httpClient = new HttpClient())
            {
                // Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                var response = await httpClient.GetAsync(APIEndPoint + "Account/" +"GetAllAccounts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    accounts = JsonConvert.DeserializeObject<List<Account>>(content);
                }
            }

            ViewBag.AccountId = new SelectList(accounts, "AccountId", "AccountId");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    // Add Token to header of Request
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                    if (!string.IsNullOrEmpty(tokenString))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                    }

                    // Send POST request
                    var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(APIEndPoint + "Order", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Orders");
                    }
                }
            }

            List<Account> accounts = new List<Account>();

            using (var httpClient = new HttpClient())
            {
                // Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                var response = await httpClient.GetAsync(APIEndPoint + "GetAllAccounts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    accounts = JsonConvert.DeserializeObject<List<Account>>(content);
                }
            }

            ViewBag.AccountId = new SelectList(accounts, "AccountId", "Username");
            return View(order);
        }





        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                #region Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                #endregion

                using (var response = await httpClient.GetAsync(APIEndPoint + "Order/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Order>(content);

                        if (result != null)
                        {
                            return View(result);
                        }
                    }
                }
            }
            return View(new Order());
        }
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                // Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                var response = await httpClient.GetAsync(APIEndPoint + "Order/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var order = JsonConvert.DeserializeObject<Order>(content);

                    if (order != null)
                    {
                        return View(order);
                    }
                }
            }

            return RedirectToAction("Index", "Orders");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                // Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                var response = await httpClient.DeleteAsync(APIEndPoint + "Order/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Orders");
                }
            }

            return RedirectToAction("Index", "Orders");
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = null;

            using (var httpClient = new HttpClient())
            {
                // Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                var response = await httpClient.GetAsync(APIEndPoint + "Order/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    order = JsonConvert.DeserializeObject<Order>(content);
                }
            }

            if (order == null)
            {
                return NotFound();
            }

            List<Account> accounts = new List<Account>();

            using (var httpClient = new HttpClient())
            {
                // Add Token to header of Request
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                var response = await httpClient.GetAsync(APIEndPoint + "Account/" +"GetAllAccounts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    accounts = JsonConvert.DeserializeObject<List<Account>>(content);
                }
            }

            ViewData["AccountId"] = new SelectList(accounts, "AccountId", "AccountId", order.AccountId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,AccountId,OrderDate,CustomerName,CustomerEmail,ShippingAddress,BillingAddress,OrderTotal,OrderStatus,PaymentMethod,PaymentStatus")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        // Add Token to header of Request
                        var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                        if (!string.IsNullOrEmpty(tokenString))
                        {
                            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenString}");
                        }

                        // Send PUT request to API
                        var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                        var response = await httpClient.PutAsync($"{APIEndPoint}Order", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // Log the error or show feedback to the user
                            ModelState.AddModelError(string.Empty, "Failed to update the order. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (logging)
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            // Fetch all accounts for dropdown
            var accounts = await FetchAllAccounts();
            ViewData["AccountId"] = new SelectList(accounts, "AccountId", "Username", order.AccountId);
            return View(order);
        }

        // Helper method to fetch all accounts
        private async Task<List<Account>> FetchAllAccounts()
        {
            var accounts = new List<Account>();

            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenString}");
                }

                var response = await httpClient.GetAsync($"{APIEndPoint}GetAllAccounts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    accounts = JsonConvert.DeserializeObject<List<Account>>(content);
                }
            }

            return accounts;
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
