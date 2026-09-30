using AutoMapper;
using BLL.DTO.User;
using BLL.Interfaces;
using DAL.EF.Table;
using DAL.Interfaces;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        private readonly IBorrowRepo _borrowRepo;

        private readonly IMapper _mapper;

        public UserService(
            IUserRepo userRepo,
            IBorrowRepo borrowRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;

            _borrowRepo = borrowRepo;

            _mapper = mapper;
        }


        
        public async Task<List<UserListDTO>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();

            return _mapper.Map<List<UserListDTO>>(users);
        }


        
        public async Task<UserDetailsDTO?> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid user id.");
            }

            var user = await _userRepo.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return _mapper.Map<UserDetailsDTO>(user);
        }


        
        public async Task CreateUserByAdminAsync(AdminCreateUserDTO dto)
        {
            
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Name is required.");
            }

            if (dto.Name.Length < 2)
            {
                throw new Exception("Name is too short.");
            }


            
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new Exception("Email is required.");
            }

            bool emailExists =
                await _userRepo.GetByEmailAsync(dto.Email) != null;

            if (emailExists)
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


            
            var validRoles = new List<string>
            {
                "Admin",
                "Librarian",
                "Member"
            };

            if (!validRoles.Contains(dto.Role))
            {
                throw new Exception("Invalid role.");
            }


            
            var user = _mapper.Map<User>(dto);

            
            user.PasswordHash = dto.Password;

            user.CreatedAt = DateTime.Now;


            
            await _userRepo.AddAsync(user);

            await _userRepo.SaveAsync();
        }


        
        public async Task UpdateUserAsync(UserUpdateDTO dto)
        {
            if (dto.Id <= 0)
            {
                throw new Exception("Invalid user id.");
            }

            var existingUser = await _userRepo.GetByIdAsync(dto.Id);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }


            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Name is required.");
            }


            
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new Exception("Email is required.");
            }

            var users = await _userRepo.GetAllAsync();

            bool duplicateEmail = users.Any(u =>
                u.Id != dto.Id &&
                u.Email.ToLower() == dto.Email.ToLower());

            if (duplicateEmail)
            {
                throw new Exception("Another user already uses this email.");
            }


            
            var validRoles = new List<string>
            {
                "Admin",
                "Librarian",
                "Member"
            };

            if (!validRoles.Contains(dto.Role))
            {
                throw new Exception("Invalid role.");
            }


            
            var passwordHash = existingUser.PasswordHash;

            var createdAt = existingUser.CreatedAt;


            
            _mapper.Map(dto, existingUser);

            existingUser.PasswordHash = passwordHash;

            existingUser.CreatedAt = createdAt;


            
            await _userRepo.UpdateAsync(existingUser);

            await _userRepo.SaveAsync();
        }


        
        public async Task DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid user id.");
            }

            var existingUser = await _userRepo.GetByIdAsync(id);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }


            
            var borrows = await _borrowRepo.GetAllAsync();

            bool hasActiveBorrow = borrows.Any(b =>
                b.UserId == id &&
                b.Status != "Returned");

            if (hasActiveBorrow)
            {
                throw new Exception("Cannot delete user with active borrowed books.");
            }


            
            await _userRepo.DeleteAsync(id);

            await _userRepo.SaveAsync();
        }
    }
}