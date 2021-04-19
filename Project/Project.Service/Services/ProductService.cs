using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
        }

        public async Task<List<Product>> ListProducts(bool loadCategories)
        {
            var products = await _productRepository.GetAll();

            var result = products.ToList();

            if (loadCategories)
                foreach(var p in result)
                    p.Category = await _categoryService.GetCategoryById(p.Category.Id);
            else
                result.ForEach(p => p.Category = null);

            return result;
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _productRepository.GetById(id);

            if (product != null) product.Category = await _categoryService.GetCategoryById(id);

            return product;
        }

        public Task<int> CreateProduct(Product entity)
            => _productRepository.Insert(entity);

        public Task<bool> UpdateProduct(Product entity)
            => _productRepository.Update(entity);

        public Task<bool> DeleteProduct(Product entity)
            => _productRepository.Delete(entity);
    }
}
