using System;
using System.Collections.Generic;
using System.Text;

namespace Tienda365.BL.Models
{
    public class CartBL
    {
        public List<CartItemBL> CartItems { get; set; } = new List<CartItemBL>();
        public int NumberOfProducts { get; set; }
        public double TotalAmount { get; set; }
    }
}
