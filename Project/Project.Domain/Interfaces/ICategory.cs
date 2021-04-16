using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> ListCategories();

        Category GetCategoryById(int id);

        void CreateCategory(Category entity);

        void UpdateCategory(Category entity);

        void DeleteCategory(Category entity);
    }

    public interface ICategoryRepository : IRepository<Category> { }
}
