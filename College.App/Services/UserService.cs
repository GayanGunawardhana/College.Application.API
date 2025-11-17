using AutoMapper;
using College.App.Data;
using College.App.Data.Repository;
using College.App.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Cryptography;

namespace College.App.Services
{
    public class UserService : IUserService
    {
        private readonly ICollegeRepository<User> _UserRepository;
        private readonly IMapper _Mapper;

        public UserService(IMapper mapper, ICollegeRepository<User> UserRepository)
        {
            _Mapper = mapper;
            _UserRepository = UserRepository;

        }
        //Returns a tuple containing the password hash and salt
        public (string PasswordHash, string Salt) CreatePasswordHash(string password)
        {
            //Create the salt
            var salt = new byte[120 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);

            }
            //Create Password Hash
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                ));


            return (hash, Convert.ToBase64String(salt));
        }
        public async Task<bool> CreateUserAsync(UserDTO dto)
        {
            
            ArgumentNullException.ThrowIfNull(dto, $"The argument {nameof(dto)} is null");


            var user = _Mapper.Map<User>(dto);
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            //Generate Password Hash and Salt
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var (passwordHash, salt) = CreatePasswordHash(dto.Password); // deconstructing tuple
                user.Password = passwordHash;
                user.PasswordSalt = salt;
            }

            await _UserRepository.CreateStudentAsync(user);


            return true;

        }
        public async Task<UserReadOnlyDTO> GetUseerAsync()
        {
            var user = await _UserRepository.GetAllByFilterAsync(e => !e.IsDelete);
            var userDto = _Mapper.Map<UserReadOnlyDTO>(user);
            return userDto;
        }
        public async Task<UserReadOnlyDTO> GetUserById(int Id)
        {
            var user = await _UserRepository.GetStudentByIdAsync(u => u.Id == Id && !u.IsDelete, true);
            var userDto = _Mapper.Map<UserReadOnlyDTO>(user);
            return userDto;
        }
        public async Task<UserReadOnlyDTO> GetUserByUsername(string Username)
        {             
            var user = await _UserRepository.GetStudentByNameAsync(u => u.Username == Username && !u.IsDelete);
            var userDto = _Mapper.Map<UserReadOnlyDTO>(user);
            return userDto;
        }
        public async Task<bool> UpdateUserAsync(UserDTO DTO)
        {
            ArgumentNullException.ThrowIfNull(DTO, $"The argument {nameof(DTO)} is null");
            var user = await _UserRepository.GetStudentByIdAsync(u => u.Id == DTO.Id && !u.IsDelete, true);
            if (user == null)
            {
                throw new ArgumentException($"User with Id {DTO.Id} not found");
            }
            var newUser= _Mapper.Map<User>(DTO);
            newUser.ModifiedDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(DTO.Password))
            {
                var (passwordHash, salt) = CreatePasswordHash(DTO.Password); // deconstructing tuple
                newUser.Password = passwordHash;
                newUser.PasswordSalt = salt;
            }

            await  _UserRepository.UpdateStudentAsync(newUser);
            return true;

        }
        public async Task<bool> DeleteUserAsync(int Id)
        {
            // This is a soft delete method
            if (Id <= 0)
            {
                throw new ArgumentException($"Invalid Id {Id}");
            }
            var user = await _UserRepository.GetStudentByIdAsync(u => u.Id == Id && !u.IsDelete, true);
            if (user == null)
            {
                throw new ArgumentException($"User with Id {Id} not found");
            }
            user.IsDelete = true;
            user.ModifiedDate = DateTime.Now;
            //We don't want to delete this records from database, just mark it as deleted
            await _UserRepository.UpdateStudentAsync(user);
            return true;
        }


    }
}
