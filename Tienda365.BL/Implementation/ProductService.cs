using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Interface;
using Tienda365.DL.Entities;
using Tienda365.DL.Repositories;

namespace Tienda365.BL.Implementation
{
    public class ProductService : IProductService
    {
        private IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            this._productRepo = productRepo;
        }

        public async Task<bool> AddNewProduct(ProductBL newProduct)
        {
            if (DateTime.UtcNow.Month == 9 || DateTime.UtcNow.Month == 10)
            {
                newProduct.MaxOrderAmount = 2;
            }
            var newEntity = new Product
            {
                CategoryId = newProduct.CategoryId,
                Name = newProduct.Name,
                MRPAmount = newProduct.MRPAmount,
                InStock = newProduct.InStock,
                MaxOrderAmount = newProduct.MaxOrderAmount
            };
            return await _productRepo.AddNewProduct(newEntity);
        }

        public async Task<List<Product>> GetProducts(int limit = 10, int page = 1)
        {
            return await _productRepo.GetProducts(limit, page);
        }

        public async Task<List<Product>> GetProductsByCategory(int CategoryId, int limit = 10, int page = 1)
        {
            return await _productRepo.GetProductsByCategory(CategoryId, limit, page);
        }

        public async Task<List<Product>> GetProductsByName(string name, int limit = 10, int page = 1)
        {
            return await _productRepo.GetProductsByName(name);
        }
    }
}
