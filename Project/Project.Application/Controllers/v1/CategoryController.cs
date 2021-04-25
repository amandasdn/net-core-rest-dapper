using Microsoft.AspNetCore.Authorization;
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
    /// Category Controller.
    /// </summary>
    [Authorize, ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUser _user;
        private readonly ILogger _logger;

        /// <summary>
        /// API: Category
        /// </summary>
        public CategoryController(ICategoryService categoryService, IUser user, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _user = user;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<List<Category>>), 200)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] bool onlyActive = true)
        {
            _logger.LogDebug("Category.Get", $"OnlyActive: {onlyActive}");

            var response = new Response<List<Category>>();

            try
            {
                var result = await _categoryService.ListCategories();

                response.Data = result.Where(x => (!onlyActive || x.Active)).ToList();

                _logger.LogInformation($"Listagem de categorias obtida com sucesso.");

                return Ok(response);
            }
            catch(Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }

        /// <summary>
        /// Get category by id.
        /// </summary>
        [ProducesResponseType(typeof(Response<Category>), 200)]
        [ProducesResponseType(typeof(Response<object>), 404)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<Category>>> GetAsync([FromRoute, Required] int id)
        {
            var response = new Response<Category>();

            try
            {
                var category = await _categoryService.GetCategoryById(id);

                if (category == null)
                {
                    response.SetError("Não há nenhuma categoria com o ID especificado.");
                    return NotFound(response);
                }

                response.Data = category;

                _logger.LogInformation($"Categoria [{category.Name}] obtida com sucesso.");

                return Ok(response);
            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }

        /// <summary>
        /// Create a category.
        /// </summary>
        [ProducesResponseType(typeof(Response<Category>), 201)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpPost]
        public async Task<ActionResult<Response<object>>> CreateAsync([FromForm] CategoryRequest request)
        {
            var response = new Response<object>();

            try
            {
                Category category = new Category
                {
                    Name = request.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(request.Description) ? string.Empty : request.Description.Trim()
                };

                var resultId = await _categoryService.CreateCategory(category);

                if (resultId <= 0)
                    throw new Exception("Ocorreu um erro ao tentar cadastrar a categoria.");

                category.Id = resultId;

                response.Data = category;

                _logger.LogInformation($"Categoria [{category.Name}] criada com sucesso.");

                return Created(nameof(CreateAsync), response);
            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }

        /// <summary>
        /// Edit a category.
        /// </summary>
        [ProducesResponseType(typeof(Response<object>), 200)]
        [ProducesResponseType(typeof(Response<object>), 404)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpPut("{id:int}/edit")]
        public async Task<ActionResult<Response<object>>> UpdateAsync([FromRoute, Required] int id, [FromForm] CategoryRequest request, [FromQuery] bool active = true)
        {
            var response = new Response<object>();

            try
            {
                var category = await _categoryService.GetCategoryById(id);

                if (category == null || category?.Id <= 0)
                {
                    response.SetError("A categoria não foi encontrada.");
                    return NotFound(response);
                }

                category.Active = active;
                category.Name = request.Name != null && request.Name?.Trim() != string.Empty ? request.Name : category.Name;
                category.Description = request.Description ?? category.Description;

                var result = await _categoryService.UpdateCategory(category);

                if (!result)
                    throw new Exception("Ocorreu um erro ao tentar cadastrar a categoria.");

                _logger.LogInformation($"Categoria [{category.Name}] alterada com sucesso.");

                return response;
            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }

        /// <summary>
        /// Set a category as removed.
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
                var category = await _categoryService.GetCategoryById(id);

                if (category == null || category?.Id <= 0)
                {
                    response.SetError("A categoria não foi encontrada.");
                    return NotFound(response);
                }

                var result = await _categoryService.DeleteCategory(category);

                if (!result)
                    throw new Exception("Ocorreu um erro ao tentar cadastrar a categoria.");

                _logger.LogInformation($"Categoria [{category.Name}] removida com sucesso.");

                return response;
            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }
    }
}
