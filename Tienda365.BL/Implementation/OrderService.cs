using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Interface;
using Tienda365.DL.Entities;
using Tienda365.DL.Repositories.Repo_Interfaces;

namespace Tienda365.BL.Implementation
{
    public class OrderService : IOrderService
    {
        private IOrderRepo _orderRepo;
        public OrderService(IOrderRepo orderRepo)
        {
            this._orderRepo = orderRepo;
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _orderRepo.GetOrders();
        }

        public async Task<bool> PlaceOrder()
        {
            return await _orderRepo.PlaceOrder();
        }
    }
}
