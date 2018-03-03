using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RomansShop.Core;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.WebApi.Filters;

namespace RomansShop.WebApi.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Get All Categories
        ///     api/categories
        /// </summary>
        /// <returns>List of Categories</returns>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();
            IEnumerable<CategoryResponse> categoryResponse = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponse>>(categories);

            return Ok(categoryResponse);
        }

        /// <summary>
        ///     Get Category by Id
        ///     api/categories/{id}
        /// </summary>
        /// <returns>Category</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            ValidationResponse<Category> validationResponse = _categoryService.GetById(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            CategoryResponse categoryResponse = _mapper.Map<Category, CategoryResponse>(validationResponse.ResponseData);

            return Ok(categoryResponse);
        }

        /// <summary>
        ///     Add new Category
        ///     api/categories
        /// </summary>
        /// <returns>Added Category</returns>
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody]CategoryRequest categoryRequest)
        {
            Category category = _mapper.Map<CategoryRequest, Category>(categoryRequest);
            ValidationResponse<Category> validationResponse = _categoryService.Add(category);

            if (validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            CategoryResponse categoryResponse = _mapper.Map<Category, CategoryResponse>(validationResponse.ResponseData);

            return CreatedAtAction("Get", new { id = categoryResponse.Id }, categoryResponse);
        }

        /// <summary>
        ///     Update Category
        ///     api/categories/{id}
        /// </summary>
        /// <returns>Category Object</returns>
        [HttpPut("{id}")]
        [ValidateModel]
        public IActionResult Put(Guid id, [FromBody]CategoryRequest categoryRequest)
        {
            Category category = _mapper.Map<CategoryRequest, Category>(categoryRequest);
            category.Id = id;

            ValidationResponse<Category> validationResponse = _categoryService.Update(category);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            if (validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            CategoryResponse categoryResponse = _mapper.Map<Category, CategoryResponse>(validationResponse.ResponseData);

            return Ok(categoryResponse);
        }

        /// <summary>
        ///     Delete Category by Id
        ///     api/categories/{id}
        /// </summary>
        /// <returns>Category Object</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ValidationResponse<Category> validationResponse = _categoryService.Delete(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            if(validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            return Ok(validationResponse.Message);
        }
    }
}