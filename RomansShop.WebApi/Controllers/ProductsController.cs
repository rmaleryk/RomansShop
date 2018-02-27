using System;
using System.Collections.Generic;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IProductRepository productRepository, IMapper mapper)
        {
            _productService = productService;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Get All Products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            IEnumerable<ProductResponse> response = _mapper.Map<IEnumerable<ProductResponse>>(products);

            return Ok(response);
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

            ProductResponse response = _mapper.Map<ProductResponse>(product);

            return Ok(response);
        }

        /// <summary>
        ///     Add new Product
        /// </summary>
        /// <returns>Added Product</returns>
        [HttpPost]
        public IActionResult Post([FromBody]CreateProductRequest product)
        {
            if (product == null || !ModelState.IsValid)
            {
                return BadRequest("Product is not valid.");
            }

            Product entity = _mapper.Map<Product>(product);
            entity = _productRepository.Add(entity);

            ProductResponse response = _mapper.Map<ProductResponse>(entity);

            return CreatedAtAction("Post", response);
        }

        /// <summary>
        ///     Update Product
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpPut]
        public IActionResult Put([FromBody]EditProductRequest product)
        {
            if (product == null || !ModelState.IsValid)
            {
                return BadRequest("Product is not valid.");
            }

            Product entity = _productRepository.GetById(product.Id);

            if (entity == null)
            {
                return NotFound("Product not found.");
            }

            entity = _mapper.Map<Product>(product);
            entity = _productRepository.Update(entity);

            ProductResponse response = _mapper.Map<ProductResponse>(entity);

            return Ok(response);
        }

        /// <summary>
        ///     Delete Product by Id
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpDelete]
        public IActionResult Delete([FromBody]EditProductRequest product)
        {
            if (product == null || !ModelState.IsValid)
            {
                return BadRequest("Product is not valid.");
            }

            Product entity = _productRepository.GetById(product.Id);

            if (entity == null)
            {
                return NotFound("Product not found.");
            }

            entity = _mapper.Map<Product>(product);
            _productRepository.Delete(entity);

            return Ok();
        }
    }
}
