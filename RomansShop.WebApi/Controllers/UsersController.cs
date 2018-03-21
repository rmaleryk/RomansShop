using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.WebApi.ClientModels.User;
using RomansShop.WebApi.Filters;

namespace RomansShop.WebApi.Controllers
{
    [Route("api/users")]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IUserRepository userRepository, IMapper mapper)
        {
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // api/users
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<User> users = _userRepository.GetAll();
            IEnumerable<UserResponseModel> userResponse = _mapper.Map<IEnumerable<User>, IEnumerable<UserResponseModel>>(users);

            return Ok(userResponse);
        }

        // api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            ValidationResponse<User> validationResponse = _userService.GetById(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            UserResponseModel userResponse = _mapper.Map<User, UserResponseModel>(validationResponse.ResponseData);

            return Ok(userResponse);
        }

        // api/users/group/{rights}
        [HttpGet("groups/{rights}")]
        public IActionResult GetByRights(UserRights rights)
        {
            ValidationResponse<IEnumerable<User>> validationResponse = _userService.GetByRights(rights);
            IEnumerable<UserResponseModel> userResponse = _mapper.Map<IEnumerable<User>, IEnumerable<UserResponseModel>>(validationResponse.ResponseData);

            return Ok(userResponse);
        }

        // api/users
        [HttpPost]
        public IActionResult Post([FromBody]AddUserRequestModel userRequest)
        {
            User user = _mapper.Map<AddUserRequestModel, User>(userRequest);
            ValidationResponse<User> validationResponse = _userService.Add(user);

            if (validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            UserResponseModel userResponse = _mapper.Map<User, UserResponseModel>(validationResponse.ResponseData);

            return CreatedAtAction("GetById", new { id = userResponse.Id }, userResponse);
        }

        // api/users/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]UpdateUserRequestModel userRequest)
        {
            User user = _mapper.Map<UpdateUserRequestModel, User>(userRequest);
            user.Id = id;

            ValidationResponse<User> validationResponse = _userService.Update(user);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            if (validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            UserResponseModel userResponse = _mapper.Map<User, UserResponseModel>(validationResponse.ResponseData);

            return Ok(userResponse);
        }

        // api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ValidationResponse<User> validationResponse = _userService.Delete(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            return Ok(validationResponse.Message);
        }

        // api/authenticate
        [HttpPost("/api/authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel authenticateModel)
        {
            ValidationResponse<User> validationResponse = 
                _userService.Authenticate(authenticateModel.Email, authenticateModel.Password);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            if(validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            UserResponseModel userResponse = _mapper.Map<User, UserResponseModel>(validationResponse.ResponseData);

            return Ok(userResponse);
        }
    }
}