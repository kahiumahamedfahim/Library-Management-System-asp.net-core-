using AutoMapper;
using BLL.DTO.Auth;
using BLL.Interfaces;
using DAL.EF.Table;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;

        private readonly IMapper _mapper;

        private readonly PasswordHasher<User> _passwordHasher;


        public AuthService(
            IUserRepo userRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;

            _mapper = mapper;

            _passwordHasher =
                new PasswordHasher<User>();
        }


        public async Task RegisterAsync(RegisterDTO dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Name is required.");
            }


            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new Exception("Email is required.");
            }


            var existingUser =
                await _userRepo.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new Exception("Email already exists.");
            }


            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new Exception("Password is required.");
            }

            if (dto.Password.Length < 6)
            {
                throw new Exception("Password must be at least 6 characters.");
            }



            if (dto.Password != dto.ConfirmPassword)
            {
                throw new Exception("Passwords do not match.");
            }



            var user =
                _mapper.Map<User>(dto);

            user.Role = "Member";

            user.CreatedAt = DateTime.Now;


            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password);



            await _userRepo.AddAsync(user);

            await _userRepo.SaveAsync();
        }



        public async Task<User?> LoginAsync(LoginDTO dto)
        {

            var user =
                await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }


            var result =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password);

            if (result ==
                PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid email or password.");
            }

            return user;
        }
        public async Task CreateUserAsync(
    string name,
    string email,
    string password,
    string role)
        {

            var exists = await _userRepo
                .GetByEmailAsync(email);

            if (exists != null)
            {
                throw new Exception(
                    "Email already exists.");
            }



            var validRoles = new[]
            {
        "Admin",
        "Librarian"
    };

            if (!validRoles.Contains(role))
            {
                throw new Exception(
                    "Invalid role.");
            }



            var hashedPassword =
                _passwordHasher.HashPassword(
                    null!,
                    password);


            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = hashedPassword,
                Role = role,
                CreatedAt = DateTime.Now
            };


            await _userRepo.AddAsync(user);

            await _userRepo.SaveAsync();
        }
    }
}