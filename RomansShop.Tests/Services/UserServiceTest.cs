using System;
using System.Collections.Generic;
using Moq;
using ILoggerFactory = RomansShop.Core.Extensibility.Logger.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.Logger.ILogger;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services;
using RomansShop.Tests.Common;
using Xunit;
using System.Linq;

namespace RomansShop.Tests.Services
{
    public class UserServiceTest : UnitTestBase
    {
        private Mock<IUserRepository> _mockRepository;
        private Mock<ILogger> _mockLogger;
        private UserService _userService;

        private static readonly Guid _userId = new Guid("00000000-0000-0000-0000-000000000001");
        private static readonly UserRights _userRights = UserRights.Administrator;

        const string GetByIdMethodName = nameof(UserService.GetById) + ". ";
        const string GetByRightsMethodName = nameof(UserService.GetByRights) + ". ";
        const string AddMethodName = nameof(UserService.Add) + ". ";
        const string UpdateMethodName = nameof(UserService.Update) + ". ";
        const string DeleteMethodName = nameof(UserService.Delete) + ". ";
        const string AuthenticateMethodName = nameof(UserService.Authenticate) + ". ";

        public UserServiceTest()
        {
            _mockRepository = MockRepository.Create<IUserRepository>();
            _mockLogger = MockRepository.Create<ILogger>();

            Mock<ILoggerFactory> mockLoggerFactory = MockRepository.Create<ILoggerFactory>();

            mockLoggerFactory
                .Setup(lf => lf.CreateLogger(typeof(UserService)))
                .Returns(_mockLogger.Object);

            _userService = new UserService(_mockRepository.Object, mockLoggerFactory.Object);
        }

        [Fact(DisplayName = GetByIdMethodName)]
        public void GetByIdTest()
        {
            User user = GetUser();

            _mockRepository
                .Setup(repo => repo.GetById(_userId))
                .Returns(user);

            ValidationResponse<User> actual = _userService.GetById(_userId);
            Guid actualId = actual.ResponseData.Id;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(_userId, actualId);
        }

        [Fact(DisplayName = GetByIdMethodName + "User not found")]
        public void GetByIdUserNotFoundTest()
        {
            string expectedMessage = $"User with id {_userId} not found.";

            _mockLogger.Setup(logger => logger.LogWarning(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetById(_userId))
                .Returns(() => null);

            ValidationResponse<User> actual = _userService.GetById(_userId);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = GetByRightsMethodName)]
        public void GetByRightsTest()
        {
            IEnumerable<User> users = new List<User> { GetUser() };

            _mockRepository
                .Setup(repo => repo.GetByRights(_userRights))
                .Returns(users);

            ValidationResponse<IEnumerable<User>> actual = _userService.GetByRights(_userRights);
            UserRights actualRights = actual.ResponseData.ElementAt(0).Rights;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(_userRights, actualRights);
        }

        [Fact(DisplayName = AddMethodName)]
        public void AddTest()
        {
            User user = GetUser();

            _mockRepository
                .Setup(repo => repo.GetByEmail(user.Email))
                .Returns(() => null);

            _mockRepository
                .Setup(repo => repo.Add(user))
                .Returns(user);

            ValidationResponse<User> actual = _userService.Add(user);
            string actualName = actual.ResponseData.FullName;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(user.FullName, actualName);
        }

        [Fact(DisplayName = AddMethodName + "Duplicate user")]
        public void AddDuplicateUserTest()
        {
            User user = GetUser();
            User duplicateUser = new User { Email = user.Email };

            string expectedMessage = $"User with email \"{user.Email}\" already exist.";

            _mockLogger.Setup(logger => logger.LogWarning(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetByEmail(user.Email))
                .Returns(duplicateUser);

            ValidationResponse<User> actual = _userService.Add(user);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        [Fact(DisplayName = UpdateMethodName)]
        public void UpdateTest()
        {
            User user = GetUser();

            _mockRepository
                .Setup(repo => repo.GetById(_userId))
                .Returns(user);

            _mockRepository
                .Setup(repo => repo.GetByEmail(user.Email))
                .Returns(() => null);

            _mockRepository
                .Setup(repo => repo.Update(user))
                .Returns(user);

            ValidationResponse<User> actual = _userService.Update(user);
            Guid actualEmail = actual.ResponseData.Id;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(user.Id, actualEmail);
        }

        [Fact(DisplayName = UpdateMethodName + "User not found")]
        public void UpdateUserNotFoundTest()
        {
            User user = GetUser();

            string expectedMessage = $"User with id {_userId} not found.";

            _mockLogger.Setup(logger => logger.LogWarning(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetById(_userId))
                .Returns(() => null);

            ValidationResponse<User> actual = _userService.Update(user);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = UpdateMethodName + "Duplicate user")]
        public void UpdateDuplicateUserTest()
        {
            User user = GetUser();
            User duplicateUser = new User { Email = user.Email };

            string expectedMessage = $"User with email \"{user.Email}\" already exist.";

            _mockLogger.Setup(logger => logger.LogWarning(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetById(_userId))
                .Returns(user);

            _mockRepository
                .Setup(repo => repo.GetByEmail(user.Email))
                .Returns(duplicateUser);

            ValidationResponse<User> actual = _userService.Update(user);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        [Fact(DisplayName = DeleteMethodName)]
        public void DeleteTest()
        {
            User user = GetUser();
            List<Product> emptyProductsList = new List<Product>();

            _mockRepository
                .Setup(repo => repo.GetById(_userId))
                .Returns(user);

            _mockRepository.Setup(repo => repo.Delete(user));

            ValidationResponse<User> actual = _userService.Delete(_userId);
            string actualName = actual.ResponseData.FullName;

            Assert.Equal(user.FullName, actualName);
            Assert.Equal(ValidationStatus.Ok, actual.Status);
        }

        [Fact(DisplayName = DeleteMethodName + "User not found")]
        public void DeleteUserNotFoundTest()
        {
            string expectedMessage = $"User with id {_userId} not found.";

            _mockLogger.Setup(logger => logger.LogWarning(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetById(_userId))
                .Returns(() => null);

            ValidationResponse<User> actual = _userService.Delete(_userId);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = AuthenticateMethodName)]
        public void AuthenticateTest()
        {
            string email = "test@test.com";
            string password = "passwordHash";

            User user = GetUser();

            _mockRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns(user);

            ValidationResponse<User> actual = _userService.Authenticate(email, password);

            Assert.Equal(email, actual.ResponseData.Email);
            Assert.Equal(ValidationStatus.Ok, actual.Status);
        }

        [Fact(DisplayName = AuthenticateMethodName + "Email not found")]
        public void AuthenticateEmailNotFoundTest()
        {
            string email = "failedtest@test.com";
            string password = "passwordHash";

            string expectedMessage = $"User with email {email} does not exist.";

            _mockRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns(() => null);

            _mockLogger.Setup(logger => logger.LogWarning(expectedMessage));

            ValidationResponse<User> actual = _userService.Authenticate(email, password);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = AuthenticateMethodName + "Wrong password")]
        public void AuthenticateWrongPasswordTest()
        {
            string email = "test@test.com";
            string password = "wrongPasswordHash";

            string expectedMessage = $"Wrong password for email {email}.";
            User user = GetUser();

            _mockRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns(user);

            _mockLogger.Setup(logger => logger.LogWarning(expectedMessage));

            ValidationResponse<User> actual = _userService.Authenticate(email, password);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.Failed, actual.Status);
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
    }
}