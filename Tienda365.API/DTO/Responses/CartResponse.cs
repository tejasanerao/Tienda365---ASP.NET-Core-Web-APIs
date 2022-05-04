using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda365.API.Models;
using Tienda365.BL;

namespace Tienda365.API.DTO.Responses
{
    public class CartResponse
    {
        public List<CartItemBL> CartItem { get; set; } = new List<CartItemBL>();
        public int NumberOfProducts { get; set; }
        public int TotalAmount { get; set; }
    }
}
