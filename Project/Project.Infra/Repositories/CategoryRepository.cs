using Dapper;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Project.Infra.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
            => _connectionString = configuration.GetConnectionString("Mands01");

        public IEnumerable<Category> GetAll()
        {
            IEnumerable<Category> categories;

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @" 
                    SELECT * FROM TB_CATEGORY;
                ";

                categories = con.Query<dynamic>(query)
                .Select(item => new Category
                {
                    Id = item.ID_CATEGORY,
                    CreatedOn = item.DT_CREATED_ON,
                    Active = item.FL_ACTIVE,
                    Removed = item.FL_REMOVED,
                    Name = item.DS_NAME,
                    Description = item.DS_DESCRIPTION
                });
            };

            return categories;
        }

        public Category GetById(int id)
        {
            Category category;

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @" SELECT * FROM TB_CATEGORY WHERE ID_CATEGORY = @ID_CATEGORY; ";
            };

            return null;
        }

        public void Insert(Category entity)
        {
            var prm = new DynamicParameters();
            prm.Add("@DS_NAME", entity.Name);
            prm.Add("@DS_DESCRIPTION", entity.Description);

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO TB_CATEGORY
                    VALUES (
                        GETDATE(),
                        1,
                        0,
                        @DS_NAME,
                        @DS_DESCRIPTION
                    );
                ";

                con.Execute(query, prm);
            };
        }

        public void Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
