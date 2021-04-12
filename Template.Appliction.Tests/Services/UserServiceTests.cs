using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Template.Application.AutoMapper;
using Template.Application.Services;
using Template.Application.ViewModels;
using Template.Domain.Entities;
using Template.Domain.Interfaces;
using Xunit;

namespace Template.Appliction.Tests.Services
{
    public class UserServiceTests
    {
        private UserService userService;

        public UserServiceTests()
        {
            userService = new UserService(new Mock<IUserRepository>().Object, new Mock<IMapper>().Object);
        }

        #region ValidatingSendingID

        [Fact]
        public void Post_SendingValidId()
        {
            var excpetion = Assert.Throws<Exception>(() => userService.Post(new UserViewModel { Id = Guid.NewGuid() }));
            Assert.Equal("UserId must be empty", excpetion.Message);
        }

        [Fact]
        public void GetBtId_SendingEmptyGuid()
        {
            var exception = Assert.Throws<Exception>(() => userService.GetById(""));
            Assert.Equal("UserID is not valid", exception.Message);
        }

        [Fact]
        public void Put_SendingEmptyGuid()
        {
            var exception = Assert.Throws<Exception>(() => userService.Put(new UserViewModel()));
            Assert.Equal("ID is invalid", exception.Message);
        }

        [Fact]
        public void Delete_SendingEmptyGuid()
        {
            var exception = Assert.Throws<Exception>(() => userService.Delete(""));
            Assert.Equal("UserID is not valid", exception.Message);
        }
        
        [Fact]
        public void Authenticate_SendingEmptyValues()
        {
            var exception = Assert.Throws<Exception>(() => userService.Authenticate(new UserAuthenticateRequestViewModel()));
            Assert.Equal("Email/Password are required.", exception.Message);
        }
        #endregion

        #region ValidatingCorrectObject
        [Fact]
        public void Post_SendingValidObject()
        {
            var result = userService.Post(new UserViewModel { Name = "Nicolas Fontes", Email = "nicolas.rfontes@gmail.com" });
            Assert.True(result);
        }

        [Fact]
        public void Get_ValidatingObject()
        {
            List<User> _users = new List<User>();
            _users.Add(new User { Id = new Guid(), Name = "Nicolas Fontes", Email = "nicolas.rfontes@gmail.com", DateCreated = DateTime.Now });

            var _userRepository = new Mock<IUserRepository>();
            _userRepository.Setup(x => x.GetAll()).Returns(_users);

            var _autoMapperProfile = new AutoMapperSetup();
            var _configuration = new MapperConfiguration(x => x.AddProfile(_autoMapperProfile));
            IMapper _mapper = new Mapper(_configuration);

            userService = new UserService(_userRepository.Object, _mapper);
            var result = userService.Get();
            Assert.True(result.Count > 0);
        }
        #endregion

        #region ValidatingRequiredFields
        [Fact]
        public void Post_SendingInvalidObject()
        {
            var exception = Assert.Throws<ValidationException>(() => userService.Post(new UserViewModel { Name = "Nicolas Fontes" }));
            Assert.Equal("The Email field is required.", exception.Message);
        }
        #endregion
    }
}
