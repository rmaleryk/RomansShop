using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.WebApi.ClientModels.Category;
using RomansShop.WebApi.Filters;

namespace RomansShop.WebApi.Controllers
{
    [Route("api/categories")]
    [TypeFilter(typeof(ValidateModelAttribute))]
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

        // api/categories
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();
            IEnumerable<CategoryResponseModel> categoryResponse = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponseModel>>(categories);

            return Ok(categoryResponse);
        }

        // api/categories/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            ValidationResponse<Category> validationResponse = _categoryService.GetById(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            CategoryResponseModel categoryResponse = _mapper.Map<Category, CategoryResponseModel>(validationResponse.ResponseData);

            return Ok(categoryResponse);
        }

        // api/categories
        [HttpPost]
        public IActionResult Post([FromBody]CategoryRequestModel categoryRequest)
        {
            Category category = _mapper.Map<CategoryRequestModel, Category>(categoryRequest);
            ValidationResponse<Category> validationResponse = _categoryService.Add(category);

            if (validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            CategoryResponseModel categoryResponse = _mapper.Map<Category, CategoryResponseModel>(validationResponse.ResponseData);

            return CreatedAtAction("Get", new { id = categoryResponse.Id }, categoryResponse);
        }

        // api/categories/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]CategoryRequestModel categoryRequest)
        {
            Category category = _mapper.Map<CategoryRequestModel, Category>(categoryRequest);
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

            CategoryResponseModel categoryResponse = _mapper.Map<Category, CategoryResponseModel>(validationResponse.ResponseData);

            return Ok(categoryResponse);
        }

        // api/categories/{id}
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