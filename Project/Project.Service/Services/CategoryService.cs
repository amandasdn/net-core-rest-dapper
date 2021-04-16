using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
            => _categoryRepository = categoryRepository;

        public IEnumerable<Category> ListCategories()
            => _categoryRepository.GetAll();

        public Category GetCategoryById(int id)
            => _categoryRepository.GetById(id);

        public void CreateCategory(Category entity)
            => _categoryRepository.Insert(entity);

        public void UpdateCategory(Category entity)
            => _categoryRepository.Update(entity);

        public void DeleteCategory(Category entity)
            => _categoryRepository.Delete(entity);
    }
}
