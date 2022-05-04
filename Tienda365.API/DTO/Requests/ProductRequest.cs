using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda365.API.DTO.Requests
{
    public class ProductRequest
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Category to which this prodcut belongs, if you dont know the category consult admin/db
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
        /// <summary>
        /// the maximum retail price
        /// </summary>
        [Required]
        public double MRP { get; set; }

        /// <summary>
        /// The discount offered
        /// </summary>
        public string Discount { get; set; }

        /// <summary>
        /// Defines if the product is in stock or not
        /// </summary>
        public bool InStock { get; set; } = true;

        /// <summary>
        /// Ther maxium amount a user can order
        /// </summary>
        public int MaxOrderAmount { get; set; } = 5;
    }
}
