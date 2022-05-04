using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.DL.Entities;

namespace Tienda365.DL.Repositories.Repo_Interfaces
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetOrders();
        Task<bool> PlaceOrder();
    }
}
