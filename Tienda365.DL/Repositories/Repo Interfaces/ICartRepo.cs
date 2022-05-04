using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.DL.Entities;
using Tienda365.DL.Models;

namespace Tienda365.DL.Repositories
{
    public interface ICartRepo
    {
        Task<bool> AddItem(Cart cart);
        Task<bool> UpdateItem(Cart cart);
        Task<bool> DeleteItem(Cart cart);
        Task<List<CartItemModel>> GetCartItems();

    }
}
