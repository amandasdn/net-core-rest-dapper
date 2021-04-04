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
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            var response = new List<Category>();

            return response;
        }
    }
}
