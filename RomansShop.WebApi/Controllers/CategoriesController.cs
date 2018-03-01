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
        /// </summary>
        /// <returns>List of Categorys</returns>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();
            IEnumerable<CategoryResponse> categoryResponse = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponse>>(categories);

            return Ok(categoryResponse);
        }

        /// <summary>
        ///     Get Category by Id
        /// </summary>
        /// <returns>Category</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            CategoryResponse categoryResponse = _mapper.Map<Category, CategoryResponse>(category);

            return Ok(categoryResponse);
        }

        /// <summary>
        ///     Add new Category
        /// </summary>
        /// <returns>Added Category</returns>
        [HttpPost]
        public IActionResult Post([FromBody]CreateCategoryRequest createCategoryRequest)
        {
            if (createCategoryRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Category is not valid.");
            }

            Category category = _mapper.Map<CreateCategoryRequest, Category>(createCategoryRequest);
            category = _categoryRepository.Add(category);

            CategoryResponse categoryResponse = _mapper.Map<Category, CategoryResponse>(category);

            return CreatedAtAction("Get", new { id = categoryResponse.Id }, categoryResponse);
        }

        /// <summary>
        ///     Update Category
        /// </summary>
        /// <returns>List of Categorys</returns>
        [HttpPut]
        public IActionResult Put([FromBody]EditCategoryRequest editCategoryRequest)
        {
            if (editCategoryRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Category is not valid.");
            }

            Category category = _categoryRepository.GetById(editCategoryRequest.Id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            category = _mapper.Map<EditCategoryRequest, Category>(editCategoryRequest);
            category = _categoryRepository.Update(category);

            CategoryResponse categoryResponse = _mapper.Map<Category, CategoryResponse>(category);

            return Ok(categoryResponse);
        }

        /// <summary>
        ///     Delete Category by Id
        /// </summary>
        /// <returns>List of Categorys</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            _categoryRepository.Delete(category);

            return Ok("Category was deleted.");
        }
    }
}