﻿using Microsoft.AspNetCore.Mvc;
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
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var response = new List<Product>();

            return response;
        }
    }
}
