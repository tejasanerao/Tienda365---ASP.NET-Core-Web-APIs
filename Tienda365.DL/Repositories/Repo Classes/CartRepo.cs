using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda365.DL.Entities;
using Tienda365.DL.Models;

namespace Tienda365.DL.Repositories
{
    public class CartRepo : ICartRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AppDbContext _dbContext;


        public CartRepo(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            this._dbContext = dbContext;
        }

        public async Task<bool> AddItem(Cart cart)
        {
            //var curUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var curUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            cart.Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == cart.ProductId);
            cart.UserId = curUserId;
            await _dbContext.Carts.AddAsync(cart);
            var result = await _dbContext.SaveChangesAsync();
            if(result == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteItem(Cart cart)
        {
            try
            {
                //var curUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                var curUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                cart.UserId = curUserId;
                Cart cartItem = await _dbContext.Carts.FirstOrDefaultAsync(x => x.ProductId == cart.ProductId && x.UserId == curUserId);
                if (cartItem == null)
                {
                    return false;
                }
                _dbContext.Carts.Remove(cartItem);
                var result = await _dbContext.SaveChangesAsync();
                if (result == 1)
                {
                    return true;
                }
                return false;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<List<CartItemModel>> GetCartItems()
        {
            //var curUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            //var id = curUser.Id;
            var curUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var cartItems = await _dbContext.Carts.Where(x => x.UserId == curUserId).ToListAsync();
            var cart = new List<CartItemModel>();
            foreach(var item in cartItems)
            {
                var product = _dbContext.Products.Where(x => x.Id == item.ProductId);
                cart.Add(new CartItemModel { Product = product.FirstOrDefault(), Count = item.Count });
            }
            return cart;
        }

        public async Task<bool> UpdateItem(Cart cart)
        {
            try
            {
                //var curUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                var curUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                Cart cartItem = await _dbContext.Carts.FirstOrDefaultAsync(x => x.ProductId == cart.ProductId && x.UserId == curUserId);
                if (cartItem == null)
                {
                    await _dbContext.Carts.AddAsync(cart);
                    var result2 = await _dbContext.SaveChangesAsync();
                    if (result2 == 1)
                    {
                        return true;
                    }
                    return false;
                }
                cartItem.Count = cart.Count;
                _dbContext.Carts.Update(cartItem);
                var result = await _dbContext.SaveChangesAsync();
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
