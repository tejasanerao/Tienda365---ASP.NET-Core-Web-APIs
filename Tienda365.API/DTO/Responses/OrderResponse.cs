using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda365.API.Models;

namespace Tienda365.API.DTO.Responses
{
    public class OrderResponse
    {
        public List<OrderItemModel> orderItems { get; set; } = new List<OrderItemModel>();
    }
}
