using GrowthTracker.Repositories;
using GrowthTracker.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthTracker.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(string id);
        Task<int> UpdateOrderAsync(Order order);
        Task<int> CreateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(string id);
        Task<List<Order>> Search(string key, string searchTerm);
    }
    public class OrderService : IOrderService
    {
        private readonly OrderRepository orderRepository;
        public OrderService() => 
            this.orderRepository = new OrderRepository();
   
        //public OrderService()
        //{
        //    this.orderRepository = new OrderRepository();
        //}
        public async Task<int> CreateOrderAsync(Order order)
        {
            return await orderRepository.CreateAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(string id)
        {
            var order = await orderRepository.GetOrderByIdAsync(int.Parse(id));
            if (order != null)
            {
                return await orderRepository.RemoveAsync(order);
            }
            return false;
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await orderRepository.GetOrderByIdAsync(int.Parse(id));
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await orderRepository.GetAllAsync();
        }

        public Task<List<Order>> Search(string key, string searchTerm)
        {
           var order = orderRepository.Search(key, searchTerm);
            if (order == null)
            {
                throw new Exception();
            }
            return order;
        }

        public async Task<int> UpdateOrderAsync(Order order)
        {   try
            {
                return await orderRepository.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
