﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Util;
using Project.Domain;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Application.Controllers
{
    /// <summary>
    /// Product Controller.
    /// </summary>
    [ApiController]
    [Route("api/v1.0/[Controller]")]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        /// <summary>
        /// API: Product
        /// </summary>
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
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
                return this.InternalServerError(response, e);
            }
        }

        /// <summary>
        /// Get product by id.
        /// </summary>
        [ProducesResponseType(typeof(Response<Product>), 200)]
        [ProducesResponseType(typeof(Response<object>), 400)]
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
                    return BadRequest(response);
                }

                response.Data = result;

                return Ok(response);
            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }

        /// <summary>
        /// Create a product.
        /// </summary>
        [ProducesResponseType(typeof(Response<object>), 201)]
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
                    product.Image.SetImage(fileImage.FileName, fileImage.ContentType, bytesToBase64);
                }

                var result = await _productService.CreateProduct(product);

                if (result <= 0)
                    throw new Exception("Ocorreu um erro ao tentar cadastrar o produto.");

                product.Id = result;

                return Created(nameof(CreateAsync), product);
            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }
    }
}