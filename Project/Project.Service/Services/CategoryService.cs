using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
            => _categoryRepository = categoryRepository;

        public Task<IEnumerable<Category>> ListCategories()
            => _categoryRepository.GetAll();

        public Task<Category> GetCategoryById(int id)
            => _categoryRepository.GetById(id);

        public Task<int> CreateCategory(Category entity)
            => _categoryRepository.Insert(entity);

        public Task<bool> UpdateCategory(Category entity)
            => _categoryRepository.Update(entity);

        public Task<bool> DeleteCategory(Category entity)
            => _categoryRepository.Delete(entity);
    }
}
