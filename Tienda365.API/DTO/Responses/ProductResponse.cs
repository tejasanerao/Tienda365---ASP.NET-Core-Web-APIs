using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tienda365.API.Models;

namespace Tienda365.API.DTO.Responses
{
    public class ProductResponse
    {
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
        //public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
