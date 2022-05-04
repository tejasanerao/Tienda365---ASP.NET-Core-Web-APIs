using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda365.DL.Entities;

namespace Tienda365.DL.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private AppDbContext _dbContext;

        public ProductRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddNewProduct(Product newProduct)
        {
            _dbContext.Products.Add(newProduct);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Product>> GetProducts(int limit = 10, int page = 1)
        {
            return await _dbContext.Products
                .Skip((page-1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId, int limit = 10, int page = 1)
        {
            return await _dbContext.Products
                .Where(x => x.CategoryId == categoryId)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByName(string name, int limit = 10, int page = 1)
        {
            return await _dbContext.Products.Where(x => x.Name.ToLower().Contains(name))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
    }
}
