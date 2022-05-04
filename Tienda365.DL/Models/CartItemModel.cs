using System;
using System.Collections.Generic;
using System.Text;
using Tienda365.DL.Entities;

namespace Tienda365.DL.Models
{
    public class CartItemModel
    {
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
