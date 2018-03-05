﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.WebApi.ClientModels.Product;
using RomansShop.WebApi.Filters;

namespace RomansShop.WebApi.Controllers
{
    [Route("api/products")]
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
        ///     api/products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            IEnumerable<ProductResponseModel> productResponse = 
                _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products);

            return Ok(productResponse);
        }

        /// <summary>
        ///     Get Page of products
        ///     api/products/page?startIndex={startIndex}&offset={offset}
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet("page")]
        public IActionResult GetRange([FromQuery]int startIndex, [FromQuery]int offset)
        {
            if (startIndex <= 0 || offset <= 0)
            {
                return BadRequest($"The start index ({startIndex}) or offset ({offset}) is incorrect.");
            }

            ValidationResponse<IEnumerable<Product>> validationResponse = _productService.GetRange(startIndex, offset);

            IEnumerable<ProductResponseModel> productResponse = 
                _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(validationResponse.ResponseData);

            return Ok(productResponse);
        }

        /// <summary>
        ///     Get Product by Id
        ///     api/products/{id}
        /// </summary>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            ValidationResponse<Product> validationResponse = _productService.GetById(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            ProductResponseModel productResponse = 
                _mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData);

            return Ok(productResponse);
        }

        /// <summary>
        ///     Add new Product
        ///     api/products
        /// </summary>
        /// <returns>Added Product</returns>
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody]ProductRequestModel createProductRequest)
        {
            Product product = _mapper.Map<ProductRequestModel, Product>(createProductRequest);
            product = _productRepository.Add(product);

            ProductResponseModel productResponse = _mapper.Map<Product, ProductResponseModel>(product);

            return CreatedAtAction("Get", new { id = productResponse.Id }, productResponse);
        }

        /// <summary>
        ///     Update Product
        ///     api/products/{id}
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpPut("{id}")]
        [ValidateModel]
        public IActionResult Put(Guid id, [FromBody]ProductRequestModel productRequest)
        {
            Product product = _mapper.Map<ProductRequestModel, Product>(productRequest);
            product.Id = id;

            ValidationResponse<Product> validationResponse = _productService.Update(product);

            if(validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            ProductResponseModel productResponse = 
                _mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData);

            return Ok(productResponse);
        }

        /// <summary>
        ///     Delete Product by Id
        ///     api/products/{id}
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ValidationResponse<Product> validationResponse = _productService.Delete(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            return Ok(validationResponse.Message);
        }

        /// <summary>
        ///     Get Products By CategoryId
        ///     api/categories/{categoryId}/products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet("/api/categories/{categoryId}/products")]
        public IActionResult GetByCategoryId(Guid categoryId)
        {
            IEnumerable<Product> products = _productRepository.GetByCategoryId(categoryId);
            IEnumerable<ProductResponseModel> productResponse = 
                _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products);

            return Ok(productResponse);
        }
    }
}