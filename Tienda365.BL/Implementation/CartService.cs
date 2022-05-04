using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Interface;
using Tienda365.BL.Models;
using Tienda365.DL.Entities;
using Tienda365.DL.Repositories;

namespace Tienda365.BL.Implementation
{
    public class CartService : ICartService
    {
        private ICartRepo _cartRepo;

        public CartService(ICartRepo cartRepo)
        {
            this._cartRepo = cartRepo;
        }

        public async Task<bool> AddItem(CartItemBL cartItem)
        {
            return await _cartRepo.AddItem(new Cart{ ProductId = cartItem.ProductId, Count = cartItem.Count });
        }

        public async Task<bool> DeleteItem(CartItemBL cartItem)
        {
            return await _cartRepo.DeleteItem(new Cart { ProductId = cartItem.ProductId});
        }

        public async Task<CartBL> GetCartItems()
        {
            var products = await _cartRepo.GetCartItems();
            //double totalAmount = 0;
            //int numberOfProducts = 0;
            var cart = new CartBL();
            foreach (var product in products)
            {
                var cartItem = new CartItemBL
                {
                    ProductId = product.Product.Id,
                    Name = product.Product.Name,
                    Image = product.Product.Image,
                    MRPAmount = product.Product.MRPAmount,
                    Count = product.Count
                };
                cart.CartItems.Add(cartItem);
                cart.TotalAmount += product.Product.MRPAmount * product.Count;
                cart.NumberOfProducts += product.Count;
            }

            return cart;
        }

        public async Task<bool> UpdateItem(CartItemBL cartItem)
        {
            return await _cartRepo.UpdateItem(new Cart { ProductId = cartItem.ProductId, Count = cartItem.Count });
        }
    }
}
