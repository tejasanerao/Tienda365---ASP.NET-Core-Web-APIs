using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.DL.Entities;

namespace Tienda365.BL.Interface
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        Task<bool> PlaceOrder();
    }
}
