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
            IEnumerable<ProductResponse> productResponse = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products);

            return Ok(productResponse);
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

            ProductResponse productResponse = _mapper.Map<Product, ProductResponse>(product);

            return Ok(productResponse);
        }

        /// <summary>
        ///     Add new Product
        /// </summary>
        /// <returns>Added Product</returns>
        [HttpPost]
        public IActionResult Post([FromBody]CreateProductRequest createProductRequest)
        {
            if (createProductRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Product is not valid.");
            }

            Product product = _mapper.Map<CreateProductRequest, Product>(createProductRequest);
            product = _productRepository.Add(product);

            ProductResponse productResponse = _mapper.Map<Product, ProductResponse>(product);

            return CreatedAtAction("Post", productResponse); // TODO: Redirect to getById
        }

        /// <summary>
        ///     Update Product
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpPut]
        public IActionResult Put([FromBody]EditProductRequest editProductRequest)
        {
            if (editProductRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Product is not valid.");
            }

            Product product = _productRepository.GetById(editProductRequest.Id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            product = _mapper.Map<EditProductRequest, Product>(editProductRequest);
            product = _productRepository.Update(product);

            ProductResponse productResponse = _mapper.Map<Product, ProductResponse>(product);

            return Ok(productResponse);
        }

        /// <summary>
        ///     Delete Product by Id
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _productRepository.Delete(product);

            return Ok("Product was deleted.");
        }
    }
}
