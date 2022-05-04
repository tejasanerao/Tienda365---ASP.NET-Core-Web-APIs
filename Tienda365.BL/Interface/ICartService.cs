using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Models;
using Tienda365.DL.Entities;

namespace Tienda365.BL.Interface
{
    public interface ICartService
    {
        Task<bool> AddItem(CartItemBL cartItem);
        Task<bool> UpdateItem(CartItemBL cartItem);
        Task<bool> DeleteItem(CartItemBL cartItem);
        Task<CartBL> GetCartItems();
    }
}
