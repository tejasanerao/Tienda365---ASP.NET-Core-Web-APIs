using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda365.API.DTO;
using Tienda365.API.DTO.Requests;
using Tienda365.API.DTO.Responses;
using Tienda365.API.Models;
using Tienda365.BL;
using Tienda365.BL.Interface;

namespace Tienda365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        private ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Get all Products
        /// </summary>
        /// <remarks>
        /// Query Parameters
        /// 
        /// </remarks>
        /// <param name="limit">Number of items per page</param>
        /// <param name="page">Current page number</param>
        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] int limit=10, int page=1)
        {
            try
            {
                var numberOfItemsPerPage = limit > 20 ? 20 : limit;
                var dbProducts = await _productService.GetProducts(numberOfItemsPerPage, page);
                var retProducts = new List<ProductModel>();
                _logger.LogTrace(limit.ToString());

                foreach (var item in dbProducts)
                {
                    retProducts.Add(new ProductModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Image = item.Image,
                        MRPAmount = item.MRPAmount,
                        Discount = item.DiscountPercentage,
                        InStock = item.InStock
                    });
                }
                return StatusCode(StatusCodes.Status200OK, new ProductResponse { Products = retProducts, CurrentPage = page });
            }
            catch(Exception ex)
            {
                var res = new Response();
                res.Message.Add("Some Error Occurred!");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
            
        }

        /// <summary>
        /// Get products by category(id)
        /// </summary>
        /// <remarks>
        /// Query Parameters
        /// 
        /// </remarks>
        /// <param name="limit">Number of items per page</param>
        /// <param name="page">Current page number</param>
        [HttpGet("category/{id}")]
        public async Task<ActionResult> GetProductbyCategory([FromRoute] int id, [FromQuery] int limit = 10, int page = 1)
        {
            try
            {
                var numberOfItemsPerPage = limit > 20 ? 20 : limit;
                var dbProducts = await _productService.GetProductsByCategory(id, numberOfItemsPerPage, page);
                var retProducts = new List<ProductModel>();
                foreach (var item in dbProducts)
                {
                    retProducts.Add(new ProductModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Image = item.Image,
                        MRPAmount = item.MRPAmount,
                        Discount = item.DiscountPercentage,
                        InStock = item.InStock
                    });
                }  // Auto mapper
                return StatusCode(StatusCodes.Status200OK, new ProductResponse { Products = retProducts, CurrentPage = page });
            }
            catch(Exception ex)
            {
                var res = new Response();
                res.Message.Add("Some Error Occurred!");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
            
        }

        /// <summary>
        /// Search product by name
        /// </summary>        
        /// <remarks>
        /// Query Parameters
        /// 
        /// </remarks>
        /// <param name="limit">Number of items per page</param>
        /// <param name="page">Current page number</param>
        [HttpGet("search")]
        public async Task<ActionResult> GetProductbyName([FromQuery] string name, int limit = 10, int page = 1)
        {
            try
            {
                var numberOfItemsPerPage = limit > 20 ? 20 : limit;
                var dbProducts = await _productService.GetProductsByName(name, numberOfItemsPerPage, page);
                var retProducts = new List<ProductModel>();
                Console.WriteLine(HttpContext.User.Identity.Name);
                foreach (var item in dbProducts)
                {
                    retProducts.Add(new ProductModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Image = item.Image,
                        MRPAmount = item.MRPAmount,
                        Discount = item.DiscountPercentage,
                        InStock = item.InStock
                    });
                }  // Auto mapper
                return StatusCode(StatusCodes.Status200OK, new ProductResponse { Products = retProducts, CurrentPage = page });
            }
            catch(Exception ex)
            {
                var res = new Response();
                res.Message.Add("Some Error Occurred!");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
            
        }


        /// <summary>
        /// Add a new product
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddNewProduct([FromBody] ProductRequest data)
        {

            try
            {
                var newProductBl = new ProductBL
                {
                    Name = data.Name,
                    MRPAmount = data.MRP,
                    CategoryId = data.CategoryId,
                    InStock = data.InStock,
                    MaxOrderAmount = data.MaxOrderAmount
                };
                var result = await _productService.AddNewProduct(newProductBl);
                _logger.LogTrace("Connected and sent data to the DB correctly");
                if (result)
                {
                    return StatusCode(StatusCodes.Status201Created);

                }
                else
                {
                    return BadRequest("Error while adding new product");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                var res = new Response();
                res.Message.Add("Some Error Occurred!");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
        }
    }
}
