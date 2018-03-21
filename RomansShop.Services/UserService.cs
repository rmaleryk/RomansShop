using System;
using System.Collections.Generic;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using ILoggerFactory = RomansShop.Core.Extensibility.Logger.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.Logger.ILogger;

namespace RomansShop.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public UserService(IUserRepository userRepository, ILoggerFactory loggerFactory)
        {
            _userRepository = userRepository;
            _loggerFactory = loggerFactory;

            _logger = _loggerFactory.CreateLogger(GetType());
        }

        public ValidationResponse<User> GetById(Guid id)
        {
            User user = _userRepository.GetById(id);

            if (user == null)
            {
                string message = $"User with id {id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<User>(ValidationStatus.NotFound, message);
            }

            return new ValidationResponse<User>(user, ValidationStatus.Ok);
        }

        public ValidationResponse<IEnumerable<User>> GetByRights(UserRights rights)
        {
            IEnumerable<User> user = _userRepository.GetByRights(rights);

            return new ValidationResponse<IEnumerable<User>>(user, ValidationStatus.Ok);
        }

        public ValidationResponse<User> Add(User user)
        {
            if (!IsUniqueEmail(user.Email))
            {
                string message = $"User with email \"{user.Email}\" already exist.";
                _logger.LogWarning(message);

                return new ValidationResponse<User>(ValidationStatus.Failed, message);
            }

            User addedUser = _userRepository.Add(user);

            return new ValidationResponse<User>(addedUser, ValidationStatus.Ok);
        }

        public ValidationResponse<User> Update(User user)
        {
            User userExistCheck = _userRepository.GetById(user.Id);

            if (userExistCheck == null)
            {
                string message = $"User with id {user.Id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<User>(ValidationStatus.NotFound, message);
            }

            User userEmailCheck = _userRepository.GetByEmail(user.Email);

            if (userEmailCheck != null && userEmailCheck.Id != user.Id)
            {
                string message = $"User with email \"{user.Email}\" already exist.";
                _logger.LogWarning(message);

                return new ValidationResponse<User>(ValidationStatus.Failed, message);
            }

            user.Password = userExistCheck.Password;

            User updatedUser = _userRepository.Update(user);

            return new ValidationResponse<User>(updatedUser, ValidationStatus.Ok);
        }

        public ValidationResponse<User> Delete(Guid id)
        {
            User user = _userRepository.GetById(id);

            if (user == null)
            {
                string message = $"User with id {id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<User>(ValidationStatus.NotFound, message);
            }

            _userRepository.Delete(user);

            return new ValidationResponse<User>(user, ValidationStatus.Ok, $"User with id {id} was deleted.");
        }

        public ValidationResponse<User> Authenticate(string email, string password)
        {
            User user = _userRepository.GetByEmail(email);

            if (user == null)
            {
                string message = $"User with email {email} does not exist.";
                _logger.LogWarning(message);

                return new ValidationResponse<User>(ValidationStatus.NotFound, message);
            }

            if (user.Password != password)
            {
                string message = $"Wrong password for email {email}.";
                _logger.LogWarning(message);

                return new ValidationResponse<User>(ValidationStatus.Failed, message);
            }

            return new ValidationResponse<User>(user, ValidationStatus.Ok);
        }

        private bool IsUniqueEmail(string email)
        {
            User user = _userRepository.GetByEmail(email);

            if (user == null)
            {
                return true;
            }

            return false;
        }
    }
}