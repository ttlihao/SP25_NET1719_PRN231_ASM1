using GrowthTracker.Repositories.Base;
using GrowthTracker.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GrowthTracker.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository()
        {

        }
        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(b => b.OrderDetails).ToListAsync();
        }
        public async Task<List<Order>> Search (string key, string searchterms)
        {
            return await _context.Orders
                .Include(b => b.OrderDetails)
                .Where(ba => 
                (ba.OrderId.ToString().Contains(key) || string.IsNullOrEmpty(key))
                && (ba.OrderDetails.FirstOrDefault().OrderDetailId.ToString().Contains(searchterms) || string.IsNullOrEmpty(searchterms))
                )
                .ToListAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(b => b.OrderDetails)
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => id == b.OrderId );
        }
    }
}
