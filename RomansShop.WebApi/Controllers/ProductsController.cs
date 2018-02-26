using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;

namespace RomansShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductService productService, IProductRepository productRepository)
        {
            _productService = productService;
            _productRepository = productRepository;
        }

        /// <summary>
        ///     Get All Products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            return Ok(products);
        }

        /// <summary>
        ///     Get Product by Id
        /// </summary>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        /// <summary>
        ///     Add new Product
        /// </summary>
        /// <returns>Added Product</returns>
        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest("Product can't be null.");
            }

            Product prod = _productRepository.Add(product);
            return CreatedAtAction("Post", prod);
        }

        /// <summary>
        ///     Update Product
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpPut]
        public IActionResult Put([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest("Product can't be null.");
            }

            Product prod = _productRepository.GetById(product.Id);

            if (prod == null)
            {
                return NotFound("Product not found.");
            }

            prod = _productRepository.Update(product);

            return Ok(prod);
        }

        /// <summary>
        ///     Delete Product by Id
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpDelete]
        public IActionResult Delete([FromBody]Product product)
        {
            Product prod = _productRepository.GetById(product.Id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _productRepository.Delete(product);

            return Ok();
        }
    }
}
