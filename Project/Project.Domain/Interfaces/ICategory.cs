using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> ListCategories();

        Task<Category> GetCategoryById(int id);

        Task<int> CreateCategory(Category entity);

        Task<bool> UpdateCategory(Category entity);

        Task<bool> DeleteCategory(Category entity);
    }

    public interface ICategoryRepository : IRepository<Category> { }
}
