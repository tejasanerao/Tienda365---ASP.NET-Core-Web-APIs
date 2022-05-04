using System;
using System.Collections.Generic;
using System.Text;

namespace Tienda365.BL
{
    public class ProductBL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double MRPAmount { get; set; }
        public int DiscountPercentage { get; set; }
        public bool InStock { get; set; }
        public int MaxOrderAmount { get; set; }
        public string InventoryId { get; set; }
        public int CategoryId { get; set; }
    }
}
