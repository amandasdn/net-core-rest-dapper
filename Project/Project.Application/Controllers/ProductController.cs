using Microsoft.AspNetCore.Mvc;
using Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Application.Controllers
{
    [ApiController]
    [Route("api/v1.0/[Controller]")]
    public class ProductController : ControllerBase
    {
        public ProductController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> ListProducts()
        {
            var response = new List<Product>();

            return response;
        }
    }
}
