using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RomansShop.Domain;
using RomansShop.Services.Extensibility;

namespace RomansShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        ///     Get All Products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var products = _productService.GetProducts();
            return new ObjectResult(products);
        }

        /// <summary>
        ///     Get Product by Id
        /// </summary>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productService.GetProduct(id);

            if (product == null)
                return NotFound();

            return new ObjectResult(product);
        }

        /// <summary>
        ///     Add new Product
        /// </summary>
        /// <returns>Added Product</returns>
        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest();

            var prod = _productService.AddProduct(product);
            return new ObjectResult(prod);
        }

        /// <summary>
        ///     Update Product
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpPut]
        public IActionResult Put([FromBody]Product product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest();

            var prod = _productService.GetProduct(product.Id);

            if (prod == null)
                return NotFound();

            _productService.UpdateProduct(product);

            return Ok();
        }

        /// <summary>
        ///     Delete Product by Id
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProduct(id);

            if (product == null)
                return NotFound();

            _productService.DeleteProduct(id);

            return Ok();
        }
    }
}
