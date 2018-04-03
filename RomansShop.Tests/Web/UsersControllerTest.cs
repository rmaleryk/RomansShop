using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.Tests.Common;
using RomansShop.WebApi.ClientModels.User;
using RomansShop.WebApi.Controllers;
using Xunit;

namespace RomansShop.Tests.Web
{
    public class UsersControllerTest : UnitTestBase
    {
        private Mock<IUserService> _mockService;
        private Mock<IUserRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private UsersController _controller;

        private static readonly UserRights _userRights = UserRights.Administrator;
        private static readonly Guid _userId = new Guid("00000000-0000-0000-0000-000000000002");

        const string GetMethodName = nameof(UsersController.Get) + ". ";
        const string GetByIdMethodName = nameof(UsersController.GetById) + ". ";
        const string GetByRightsMethodName = nameof(UsersController.GetByRights) + ". ";
        const string PostMethodName = nameof(UsersController.Post) + ". ";
        const string PutMethodName = nameof(UsersController.Put) + ". ";
        const string DeleteMethodName = nameof(UsersController.Delete) + ". ";
        const string AuthenticateMethodName = nameof(UsersController.Authenticate) + ". ";

        public UsersControllerTest()
        {
            _mockService = MockRepository.Create<IUserService>();
            _mockRepository = MockRepository.Create<IUserRepository>();
            _mockMapper = MockRepository.Create<IMapper>();

            _controller = new UsersController(_mockService.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact(DisplayName = GetMethodName)]
        public void GetTest()
        {
            IEnumerable<User> users = new List<User> { GetUser(), GetUser() };

            IEnumerable<UserResponseModel> usersResponse = 
                new List<UserResponseModel> { GetUserResponseModel(), GetUserResponseModel() };

            _mockRepository
                .Setup(repo => repo.GetAll())
                .Returns(users);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<User>, IEnumerable<UserResponseModel>>(users))
                .Returns(usersResponse);
            
            IActionResult actionResult = _controller.Get();

            OkObjectResult actual = (OkObjectResult)actionResult;
            int actualCount = ((IEnumerable<UserResponseModel>)actual.Value).Count();

            Assert.Equal(users.Count(), actualCount);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = GetByIdMethodName)]
        public void GetByIdTest()
        {
            ValidationResponse<User> validationResponse = GetOkValidationResponse();
            UserResponseModel userResponse = GetUserResponseModel();

            _mockService
                .Setup(serv => serv.GetById(_userId))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<User, UserResponseModel>(validationResponse.ResponseData))
                .Returns(userResponse);

            IActionResult actionResult = _controller.GetById(_userId);

            OkObjectResult actual = (OkObjectResult)actionResult;
            Guid actualId = ((UserResponseModel)actual.Value).Id;

            Assert.Equal(_userId, actualId);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = GetByIdMethodName + "User not found")]
        public void GetByIdUserNotFoundTest()
        {
            ValidationResponse<User> validationResponse = GetNotFoundValidationResponse();

            _mockService
                .Setup(serv => serv.GetById(_userId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.GetById(_userId);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = PostMethodName)]
        public void PostTest()
        {
            User user = GetUser();
            ValidationResponse<User> validationResponse = GetOkValidationResponse();
            AddUserRequestModel userRequest = GetAddUserRequestModel();
            UserResponseModel userResponse = GetUserResponseModel();

            _mockMapper
                .Setup(mapper => mapper.Map<AddUserRequestModel, User>(userRequest))
                .Returns(user);

            _mockService
                .Setup(serv => serv.Add(user))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<User, UserResponseModel>(validationResponse.ResponseData))
                .Returns(userResponse);

            IActionResult actionResult = _controller.Post(userRequest);

            CreatedAtActionResult actual = (CreatedAtActionResult)actionResult;
            string actualName = ((UserResponseModel)actual.Value).FullName;

            Assert.Equal(user.FullName, actualName);
            Assert.Equal(StatusCodes.Status201Created, actual.StatusCode);
        }

        [Fact(DisplayName = PostMethodName + "User already exist")]
        public void PostUserAlreadyExistTest()
        {
            User user = GetUser();
            ValidationResponse<User> validationResponse = GetFailedValidationResponse();
            AddUserRequestModel userRequest = GetAddUserRequestModel();

            _mockMapper
                .Setup(mapper => mapper.Map<AddUserRequestModel, User>(userRequest))
                .Returns(user);

            _mockService
                .Setup(serv => serv.Add(user))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Post(userRequest);

            BadRequestObjectResult actual = (BadRequestObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact(DisplayName = PutMethodName)]
        public void PutTest()
        {
            User user = GetUser();
            UpdateUserRequestModel userRequest = GetUpdateUserRequestModel();
            UserResponseModel userResponse = GetUserResponseModel();
            ValidationResponse<User> validationResponse = GetOkValidationResponse();

            _mockMapper
                .Setup(mapper => mapper.Map<UpdateUserRequestModel, User>(userRequest))
                .Returns(user);

            _mockService
                .Setup(serv => serv.Update(user))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<User, UserResponseModel>(validationResponse.ResponseData))
                .Returns(userResponse);

            IActionResult actionResult = _controller.Put(_userId, userRequest);

            OkObjectResult actual = (OkObjectResult)actionResult;
            string actualName = ((UserResponseModel)actual.Value).FullName;

            Assert.Equal(user.FullName, actualName);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = PutMethodName + "User not found")]
        public void PutUserNotFoundTest()
        {
            User user = GetUser();
            UpdateUserRequestModel userRequest = GetUpdateUserRequestModel();
            ValidationResponse<User> validationResponse = GetNotFoundValidationResponse();

            _mockMapper
                .Setup(mapper => mapper.Map<UpdateUserRequestModel, User>(userRequest))
                .Returns(user);

            _mockService
                .Setup(serv => serv.Update(user))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Put(_userId, userRequest);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = PutMethodName + "User already exist")]
        public void PutUserAlreadyExistTest()
        {
            User user = GetUser();
            UpdateUserRequestModel userRequest = GetUpdateUserRequestModel();
            ValidationResponse<User> validationResponse = GetFailedValidationResponse();

            _mockMapper
                .Setup(mapper => mapper.Map<UpdateUserRequestModel, User>(userRequest))
                .Returns(user);

            _mockService
                .Setup(serv => serv.Update(user))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Put(_userId, userRequest);

            BadRequestObjectResult actual = (BadRequestObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact(DisplayName = DeleteMethodName)]
        public void DeleteTest()
        {
            ValidationResponse<User> validationResponse = GetOkValidationResponse();

            _mockService
                .Setup(serv => serv.Delete(_userId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(_userId);

            OkObjectResult actual = (OkObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = DeleteMethodName + "User not found")]
        public void DeleteUserNotFoundTest()
        {
            ValidationResponse<User> validationResponse = GetNotFoundValidationResponse();

            _mockService
                .Setup(serv => serv.Delete(_userId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(_userId);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = AuthenticateMethodName)]
        public void AuthenticateTest()
        {
            User user = GetUser();
            ValidationResponse<User> validationResponse = GetOkValidationResponse();
            AuthenticateModel authenticateModel = GetAuthenticateModel();
            UserResponseModel userResponse = GetUserResponseModel();

            _mockService
                .Setup(serv => serv.Authenticate(authenticateModel.Email, authenticateModel.Password))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<User, UserResponseModel>(validationResponse.ResponseData))
                .Returns(userResponse);

            IActionResult actionResult = _controller.Authenticate(authenticateModel);

            OkObjectResult actual = (OkObjectResult)actionResult;
            string actualEmail = ((UserResponseModel)actual.Value).Email;

            Assert.Equal(user.Email, actualEmail);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = AuthenticateMethodName + "Email not found")]
        public void AuthenticateEmailNotFoundTest()
        {
            ValidationResponse<User> validationResponse = GetNotFoundValidationResponse();
            AuthenticateModel authenticateModel = GetAuthenticateModel();

            _mockService
                .Setup(serv => serv.Authenticate(authenticateModel.Email, authenticateModel.Password))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Authenticate(authenticateModel);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = AuthenticateMethodName + "Wrong password")]
        public void AuthenticateWrongPasswordTest()
        {
            ValidationResponse<User> validationResponse = GetFailedValidationResponse();
            AuthenticateModel authenticateModel = GetAuthenticateModel();

            _mockService
                .Setup(serv => serv.Authenticate(authenticateModel.Email, authenticateModel.Password))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Authenticate(authenticateModel);

            BadRequestObjectResult actual = (BadRequestObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact(DisplayName = GetByRightsMethodName)]
        public void GetByRightsTest() 
        {
            IEnumerable<User> expectedUsers = new List<User> { GetUser(), GetUser() };

            IEnumerable<UserResponseModel> userResponse = 
                new List<UserResponseModel> { GetUserResponseModel(), GetUserResponseModel() };

            ValidationResponse<IEnumerable<User>> validationResponse = 
                new ValidationResponse<IEnumerable<User>>(expectedUsers, ValidationStatus.Ok);

            _mockService
                .Setup(serv => serv.GetByRights(_userRights))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<User>, IEnumerable<UserResponseModel>>(validationResponse.ResponseData))
                .Returns(userResponse);

            IActionResult actionResult = _controller.GetByRights(_userRights);

            OkObjectResult actual = (OkObjectResult)actionResult;
            int actualCount = ((IEnumerable<UserResponseModel>)actual.Value).Count();

            Assert.Equal(expectedUsers.Count(), actualCount);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        private static User GetUser() =>
            new User
            {
                Id = _userId,
                FullName = "TestUser",
                Email = "test@test.com",
                Password = "passwordHash",
                Rights = _userRights
            };

        private static UserResponseModel GetUserResponseModel() =>
            new UserResponseModel
            {
                Id = _userId,
                FullName = "TestUser",
                Email = "test@test.com",
                Rights = _userRights
            };

        private static AddUserRequestModel GetAddUserRequestModel() =>
            new AddUserRequestModel
            {
                FullName = "TestUser",
                Email = "test@test.com",
                Password = "passwordHash",
            };

        private static UpdateUserRequestModel GetUpdateUserRequestModel() =>
            new UpdateUserRequestModel
            {
                FullName = "TestUser",
                Email = "test@test.com",
                Rights = _userRights
            };
        private static AuthenticateModel GetAuthenticateModel() =>
            new AuthenticateModel
            {
                Email = "test@test.com",
                Password = "passwordHash"
            };

        private ValidationResponse<User> GetOkValidationResponse() => 
            new ValidationResponse<User>(GetUser(), ValidationStatus.Ok);

        private ValidationResponse<User> GetNotFoundValidationResponse() =>
            new ValidationResponse<User>(ValidationStatus.NotFound, "Not Found");

        private ValidationResponse<User> GetFailedValidationResponse() =>
            new ValidationResponse<User>(ValidationStatus.Failed, "Failed");
    }
}