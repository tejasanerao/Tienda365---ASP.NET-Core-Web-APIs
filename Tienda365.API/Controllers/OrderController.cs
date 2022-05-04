using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda365.API.DTO;
using Tienda365.API.DTO.Responses;
using Tienda365.API.Models;
using Tienda365.BL.Interface;

namespace Tienda365.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private ILogger<OrderController> _logger;
        private IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            this._logger = logger;
            this._orderService = orderService;
        }

        /// <summary>
        /// Get all the orders of a user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var items = await _orderService.GetOrders();
            var orderItems = new List<OrderItemModel>();
            foreach(var item in items)
            {
                orderItems.Add(new OrderItemModel
                {
                    Id = item.Id,
                    OrderAmount = item.OrderAmount,
                    OrderDate = item.OrderDate
                });
            }
            return Ok(new OrderResponse { orderItems = orderItems});
        }

        /// <summary>
        /// Place the Order for a user
        /// </summary>
        [HttpGet("checkout")]
        public async Task<IActionResult> PlaceOrder()
        {
            try
            {
                var result = await _orderService.PlaceOrder();
                if (result == true)
                {
                    var res = new Response();
                    res.Message.Add("Order Successfully placed.");
                    return StatusCode(StatusCodes.Status200OK, res);
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch(Exception ex)
            {
                var res = new Response();
                res.Message.Add("Some Error Occurred!");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
           
        }
    }
}
