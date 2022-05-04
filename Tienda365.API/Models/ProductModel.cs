using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tienda365.API.Models
{
    public class ProductModel
    {
        [JsonPropertyName("productId")]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double MRPAmount { get; set; }
        public int Discount { get; set; }
        public bool InStock { get; set; }
    }
}
