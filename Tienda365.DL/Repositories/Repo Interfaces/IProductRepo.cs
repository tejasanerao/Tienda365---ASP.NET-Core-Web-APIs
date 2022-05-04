using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.DL.Entities;

namespace Tienda365.DL.Repositories
{
    public interface IProductRepo
    {
        Task<List<Product>> GetProducts(int limit = 10, int page = 1);
        Task<List<Product>> GetProductsByCategory(int categoryId, int limit = 10, int page = 1);
        Task<List<Product>> GetProductsByName(string name, int limit = 10, int page = 1);
        Task<bool> AddNewProduct(Product newProduct);
    }
}
