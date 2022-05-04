using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda365.API.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderAmount { get; set; }
    }
}
