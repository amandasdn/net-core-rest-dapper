using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Application.Extensions;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Application.Controllers.v1
{
    /// <summary>
    /// Product Controller.
    /// </summary>
    [Authorize, ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUser _user;
        private readonly ILogger _logger;

        /// <summary>
        /// API: Product
        /// </summary>
        public ProductController(IProductService productService, ICategoryService categoryService, IUser user, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _user = user;
            _logger = logger;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        [ProducesResponseType(typeof(Response<List<Product>>), 200)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] bool onlyActive = true, [FromQuery] bool loadCategories = false, [FromQuery] bool loadImages = false)
        {
            var response = new Response<List<Product>>();

            try
            {
                var result = await _productService.ListProducts(loadCategories);

                response.Data = result.Where(x => (!onlyActive || x.Active)).ToList();

                if (!loadImages)
                    response.Data.ForEach(p => p.Image = null);

                return Ok(response);
            }
            catch (Exception e)
            {
                return this.InternalServerError(_logger, response, e);
            }
        }

        /// <summary>
        /// Get product by id.
        /// </summary>
        [ProducesResponseType(typeof(Response<Product>), 200)]
        [ProducesResponseType(typeof(Response<object>), 404)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<Product>>> GetAsync([FromRoute, Required] int id)
        {
            var response = new Response<Product>();

            try
            {
                var result = await _productService.GetProductById(id);

                if (result == null)
                {
                    response.SetError("Não há nenhum produto com o ID especificado.");
                    return NotFound(response);
                }

                response.Data = result;

                return Ok(response);
            }
            catch (Exception e)
            {
                return this.InternalServerError(_logger, response, e);
            }
        }

        /// <summary>
        /// Create a product.
        /// </summary>
        [ProducesResponseType(typeof(Response<object>), 201)]
        [ProducesResponseType(typeof(Response<object>), 400)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpPost]
        public async Task<ActionResult<Response<object>>> CreateAsync([FromForm] ProductRequest request, IFormFile fileImage)
        {
            var response = new Response<object>();

            try
            {
                var category = await _categoryService.GetCategoryById(request.CategoryId);

                if (category == null)
                {
                    response.SetError("Não há nenhuma categoria com o ID especificado.");
                    return BadRequest(response);
                }

                Product product = new Product
                {
                    Name = request.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(request.Description) ? string.Empty : request.Description.Trim(),
                    UnitPrice = request.UnitPrice,
                    Quantity = request.Quantity,
                    Type = request.Type,
                    Category = category
                };

                // Set formFile as a image base 64
                if(fileImage != null)
                {
                    var fileToBytes = Helper.GetBytes(fileImage);
                    var bytesToBase64 = Convert.ToBase64String(await fileToBytes);

                    product.Image = new ProductImage();
                    product.Image.SetImage($"{DateTime.Now:yyyyMMddHHmm}-{fileImage.FileName}", fileImage.ContentType, bytesToBase64);
                }

                var result = await _productService.CreateProduct(product);

                if (result <= 0)
                    throw new Exception("Ocorreu um erro ao tentar cadastrar o produto.");

                product.Id = result;

                return Created(nameof(CreateAsync), product);
            }
            catch (Exception e)
            {
                return this.InternalServerError(_logger, response, e);
            }
        }

        /// <summary>
        /// Edit a product.
        /// </summary>
        [ProducesResponseType(typeof(Response<object>), 200)]
        [ProducesResponseType(typeof(Response<object>), 404)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpPut("{id:int}/edit")]
        public async Task<ActionResult<Response<object>>> UpdateAsync([FromRoute, Required] int id, [FromForm] ProductRequest request, [FromQuery] bool active = true)
        {
            var response = new Response<object>();

            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null || product?.Id <= 0)
                {
                    response.SetError("O produto não foi encontrado.");
                    return NotFound(response);
                }

                product.Active = active;
                product.Name = request.Name != null && request.Name?.Trim() != string.Empty ? request.Name : product.Name;
                product.Description = request.Description ?? product.Description;
                product.UnitPrice = request.UnitPrice;
                product.Quantity = request.Quantity;
                product.Type = request.Type;
                product.Category = new Category { Id = request.CategoryId };

                var result = await _productService.UpdateProduct(product);

                if (!result)
                    throw new Exception("Ocorreu um erro ao tentar cadastrar o produto.");

                return Ok(response);
            }
            catch (Exception e)
            {
                return this.InternalServerError(_logger, response, e);
            }
        }

        /// <summary>
        /// Set a product as removed.
        /// </summary>
        [ProducesResponseType(typeof(Response<object>), 200)]
        [ProducesResponseType(typeof(Response<object>), 404)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpPatch("{id:int}/remove")]
        public async Task<ActionResult<Response<object>>> RemoveAsync([FromRoute, Required] int id)
        {
            var response = new Response<object>();

            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null || product?.Id <= 0)
                {
                    response.SetError("O produto não foi encontrado.");
                    return NotFound(response);
                }

                var result = await _productService.DeleteProduct(product);

                if (!result)
                    throw new Exception("Ocorreu um erro ao tentar cadastrar o produto.");

                return Ok(response);
            }
            catch (Exception e)
            {
                return this.InternalServerError(_logger, response, e);
            }
        }
    }
}