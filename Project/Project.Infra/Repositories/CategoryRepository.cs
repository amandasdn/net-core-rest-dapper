using Dapper;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Project.Infra.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
            => _connectionString = configuration.GetConnectionString("Mands01");

        public async Task<IEnumerable<Category>> GetAll()
        {
            IEnumerable<Category> categories;

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @" 
                    SELECT * FROM TB_CATEGORY;
                ";

                var result = await con.QueryAsync<dynamic>(query);

                categories = result.Select(item => new Category
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

        public async Task<Category> GetById(int id)
        {
            Category category;

            var prm = new DynamicParameters();
            prm.Add("@ID_CATEGORY", id);

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @" SELECT * FROM TB_CATEGORY WHERE ID_CATEGORY = @ID_CATEGORY; ";

                var result = await con.QueryAsync<dynamic>(query, prm);

                category = result.Select(item => new Category
                {
                    Id = item.ID_CATEGORY,
                    CreatedOn = item.DT_CREATED_ON,
                    Active = item.FL_ACTIVE,
                    Removed = item.FL_REMOVED,
                    Name = item.DS_NAME,
                    Description = item.DS_DESCRIPTION
                }).FirstOrDefault();
            };

            return category;
        }

        public async Task<int> Insert(Category entity)
        {
            int result = 0;

            var prm = new DynamicParameters();
            prm.Add("@DS_NAME", entity.Name);
            prm.Add("@DS_DESCRIPTION", entity.Description);
            prm.Add("@DT_CREATED_ON", entity.CreatedOn);

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO TB_CATEGORY (DT_CREATED_ON, FL_ACTIVE, FL_REMOVED, DS_NAME, DS_DESCRIPTION)
                    VALUES (
                        @DT_CREATED_ON,
                        1,
                        0,
                        @DS_NAME,
                        @DS_DESCRIPTION
                    );

                    SELECT CAST(SCOPE_IDENTITY() as int);
                ";

                result = await con.ExecuteScalarAsync<int>(query, prm);
            };

            return result;
        }

        public async Task<bool> Update(Category entity)
        {
            bool result = false;

            var prm = new DynamicParameters();
            prm.Add("@ID_CATEGORY", entity.Id);
            prm.Add("@FL_ACTIVE", entity.Active ? 1 : 0);
            prm.Add("@DS_NAME", entity.Name);
            prm.Add("@DS_DESCRIPTION", entity.Description);

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE TB_CATEGORY
                    SET
                        FL_ACTIVE = @FL_ACTIVE,
                        DS_NAME = @DS_NAME,
                        DS_DESCRIPTION = @DS_DESCRIPTION
                    WHERE
                        ID_CATEGORY = @ID_CATEGORY;
                ";

                var exec = await con.ExecuteAsync(query, prm);

                result = (exec > 0);
            }

            return result;
        }

        public async Task<bool> Delete(Category entity)
        {
            bool result = false;

            var prm = new DynamicParameters();
            prm.Add("@ID_CATEGORY", entity.Id);

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE TB_CATEGORY
                    SET
                        FL_REMOVED = 1
                    WHERE
                        ID_CATEGORY = @ID_CATEGORY;
                ";

                var exec = await con.ExecuteAsync(query, prm);

                result = (exec > 0);
            }

            return result;
        }
    }
}
