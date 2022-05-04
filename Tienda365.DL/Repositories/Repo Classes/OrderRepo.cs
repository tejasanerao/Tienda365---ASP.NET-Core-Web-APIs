using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda365.DL.Entities;
using Tienda365.DL.Repositories.Repo_Interfaces;

namespace Tienda365.DL.Repositories.Repo_Classes
{
    public class OrderRepo : IOrderRepo
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AppDbContext _dbContext;

        public OrderRepo(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            this._dbContext = dbContext;
        }

        public async Task<List<Order>> GetOrders()
        {
            var curUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var curUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var orderItems = await _dbContext.Orders.Where(x => x.UserId == curUserId).ToListAsync();

            return orderItems;
        }

        public async Task<bool> PlaceOrder()
        {
            var curUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var curUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var cartItems = await _dbContext.Carts.Where(x => x.UserId == curUserId).ToListAsync();
            double totalAmount = 0;
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in cartItems)
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                var productAmount = product.MRPAmount * item.Count;
                totalAmount += (double)productAmount;
                orderItems.Add(new OrderItem { ProductId = item.ProductId, Count = item.Count, ItemAmount = (double)productAmount });
            }
            Order order = new Order { OrderAmount = totalAmount, OrderDate = DateTime.Now, UserId = curUserId };
            _dbContext.Orders.Add(order);
            var result = await _dbContext.SaveChangesAsync();
            if(result == 1)
            {
                foreach (var item in orderItems)
                {
                    item.OrderId = order.Id;
                    _dbContext.OrderItems.Add(item);
                }
                result = await _dbContext.SaveChangesAsync();

            }
            foreach(var item in cartItems)
            {
                _dbContext.Carts.Remove(item);
            }

            result += await _dbContext.SaveChangesAsync();

            if(result == orderItems.Count + cartItems.Count)
            {
                return true;
            }
                
            return false;
        }
    }
}
