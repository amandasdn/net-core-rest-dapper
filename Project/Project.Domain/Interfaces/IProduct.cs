using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> ListProducts(bool loadCategories);

        Task<Product> GetProductById(int id);

        Task<int> CreateProduct(Product entity);

        Task<bool> UpdateProduct(Product entity);

        Task<bool> DeleteProduct(Product entity);
    }

    public interface IProductRepository : IRepository<Product>
    {
    }
}
