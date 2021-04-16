using Microsoft.AspNetCore.Mvc;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Application.Controllers
{
    [ApiController]
    [Route("api/v1.0/[Controller]")]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
            =>_categoryService = categoryService;

        /// <summary>
        /// Get all categories.
        /// </summary>
        [ProducesResponseType(typeof(Response<List<Category>>), 200)]
        [HttpGet]
        public ActionResult<Response<List<Category>>> Get()
        {
            var response = new Response<List<Category>>();

            try
            {
                response.Data = _categoryService.ListCategories().ToList();

                return response;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
