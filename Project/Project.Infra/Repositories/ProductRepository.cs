using Dapper;
using Microsoft.Extensions.Configuration;
using Project.Domain.Entities;
using Project.Domain.Enums;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
            => _connectionString = configuration.GetConnectionString("Mands01");

        public async Task<IEnumerable<Product>> GetAll()
        {
            IEnumerable<Product> products;

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @" 
                    SELECT
                        *
                    FROM TB_PRODUCT
                    WHERE FL_REMOVED = 0;
                ";

                var result = await con.QueryAsync<dynamic>(query);

                products = result.Select(item => new Product
                {
                    Id = item.ID_PRODUCT,
                    CreatedOn = item.DT_CREATED_ON,
                    Active = item.FL_ACTIVE,
                    Removed = item.FL_REMOVED,
                    Name = item.DS_NAME,
                    Description = item.DS_DESCRIPTION,
                    UnitPrice = item.VL_UNIT_PRICE,
                    Quantity = item.NR_QUANTITY,
                    Type = (ProductType)item.NR_TYPE,
                    Category = new Category
                    {
                        Id = item.ID_CATEGORY
                    },
                    Image = new ProductImage
                    {
                        Name = item.DS_IMAGE_NAME,
                        Type = item.DS_IMAGE_TYPE,
                        Base64 = item.DS_IMAGE_BASE64
                    }
                });
            };

            return products;
        }

        public Task<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(Product entity)
        {
            int result = 0;

            var prm = new DynamicParameters();
            prm.Add("@DT_CREATED_ON", entity.CreatedOn);
            prm.Add("@DS_NAME", entity.Name);
            prm.Add("@DS_DESCRIPTION", entity.Description);
            prm.Add("@VL_UNIT_PRICE", entity.UnitPrice);
            prm.Add("@NR_QUANTITY", entity.Quantity);
            prm.Add("@NR_TYPE", entity.Type);
            prm.Add("@DS_IMAGE_NAME", entity.Image?.Name);
            prm.Add("@DS_IMAGE_TYPE", entity.Image?.Type);
            prm.Add("@DS_IMAGE_BASE64", entity.Image?.Base64);
            prm.Add("@ID_CATEGORY", entity.Category.Id);

            using (var con = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO TB_PRODUCT
                    (
                        DT_CREATED_ON,
                        FL_ACTIVE,
                        FL_REMOVED,
                        DS_NAME,
                        DS_DESCRIPTION,
                        VL_UNIT_PRICE,
                        NR_QUANTITY,
                        NR_TYPE,
                        DS_IMAGE_NAME,
                        DS_IMAGE_TYPE,
                        DS_IMAGE_BASE64,
                        ID_CATEGORY
                    )
                    VALUES (
                        @DT_CREATED_ON,
                        1,
                        0,
                        @DS_NAME,
                        @DS_DESCRIPTION,
                        @VL_UNIT_PRICE,
                        @NR_QUANTITY,
                        @NR_TYPE,
                        @DS_IMAGE_NAME,
                        @DS_IMAGE_TYPE,
                        @DS_IMAGE_BASE64,
                        @ID_CATEGORY
                    );

                    SELECT CAST(SCOPE_IDENTITY() as int);
                ";

                result = await con.ExecuteScalarAsync<int>(query, prm);
            };

            return result;
        }

        public Task<bool> Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
