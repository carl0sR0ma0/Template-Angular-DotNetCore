using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Template.Application.Interfaces;
using Template.Application.ViewModels;
using Template.Auth.Services;
using Template.Domain.Entities;
using Template.Domain.Interfaces;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Template.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public List<UserViewModel> Get()
        {
            List<UserViewModel> _usersViewModel = new List<UserViewModel>();

            IEnumerable<User> _users = this.userRepository.GetAll();

            _usersViewModel = mapper.Map<List<UserViewModel>>(_users);

            //foreach (var item in _users)
            //{
            //    //_usersViewModel.Add(new UserViewModel { Id = item.Id, Email = item.Email, Name = item.Name });
            //    _usersViewModel.Add(mapper.Map<UserViewModel>(item));
            //}

            return _usersViewModel;
        }

        public bool Post(UserViewModel userViewModel)
        {
            //User _user = new User
            //{
            //    Id = Guid.NewGuid(),
            //    Email = userViewModel.Email,
            //    Name = userViewModel.Name
            //};

            if (userViewModel.Id != Guid.Empty)
                throw new Exception("UserId must be empty");

            Validator.ValidateObject(userViewModel, new ValidationContext(userViewModel), true);

            User _user = mapper.Map<User>(userViewModel);
            _user.Password = EncryptPassoword(_user.Password);

            this.userRepository.Create(_user);

            return true;
        }

        public UserViewModel GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("UserID is not valid");

            User _user = this.userRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_user == null)
                throw new Exception("User not found");

            return mapper.Map<UserViewModel>(_user);
        }

        public bool Put(UserViewModel userViewModel)
        {
            if (userViewModel.Id == Guid.Empty)
                throw new Exception("ID is invalid");

            User _user = this.userRepository.Find(x => x.Id == userViewModel.Id && !x.IsDeleted);
            if (_user == null)
                throw new Exception("User not found");

            _user = mapper.Map<User>(userViewModel);
            _user.Password = EncryptPassoword(_user.Password);

            this.userRepository.Update(_user);

            return true;
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("UserID is not valid");

            User _user = this.userRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_user == null)
                throw new Exception("User not found");

            return this.userRepository.Delete(_user);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                throw new Exception("Email/Password are required.");

            user.Password = EncryptPassoword(user.Password);

            User _user = this.userRepository.Find(x => !x.IsDeleted && x.Email.ToLower() == user.Email.ToLower()
                                                     && x.Password.ToLower() == user.Password.ToLower());
            if (_user == null)
                throw new Exception("User not found");

            return new UserAuthenticateResponseViewModel(mapper.Map<UserViewModel>(_user), TokenService.GenerateToken(_user));
        }

        private string EncryptPassoword(string password)
        {
            HashAlgorithm sha = new SHA1CryptoServiceProvider();

            byte[] encryptedPassowrd = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var caracter in encryptedPassowrd)
                stringBuilder.Append(caracter.ToString("X2"));

            return stringBuilder.ToString();
        }
    }
}
