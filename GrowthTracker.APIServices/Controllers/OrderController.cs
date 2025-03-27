using GrowthTracker.Repositories.Models;
using GrowthTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GrowthTracker.APIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService orderService;
        // GET: api/<OrderController>
        public OrderController(IOrderService orderService) 
            => this.orderService = orderService;


        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        public async Task<IEnumerable<Order>> Get()
        {
            return await orderService.GetOrdersAsync();
        }
        [HttpGet("{key}/{searchTerm}")]
        [Authorize(Roles = "1,2,3")]
        public async Task<IEnumerable<Order>> Search(string key, string searchTerm)
        {
            return await orderService.Search(key, searchTerm);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "1,2,3")]
        public async Task<Order> Get(string id)
        {
            return await orderService.GetOrderByIdAsync(id);
        }

        // POST api/<OrderController>
        [HttpPost]
        [Authorize(Roles = "1,2,3")]
        public async Task<int> Post([FromBody] Order orderCreate)
        {
            return await orderService.CreateOrderAsync(orderCreate);
        }

        // PUT api/<OrderController>/5
        [HttpPut]
        [Authorize(Roles = "1")]
        public async Task<int> Put([FromBody] Order orderCreate)
        {
            return await orderService.UpdateOrderAsync(orderCreate);
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<bool> Delete(string id)
        {
            return await orderService.DeleteOrderAsync(id);
        }
    }
}
