using BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListDTO>> GetAllUsersAsync();

        Task<UserDetailsDTO?> GetUserByIdAsync(int id);

        Task CreateUserByAdminAsync(AdminCreateUserDTO dto);

        Task UpdateUserAsync(UserUpdateDTO dto);

        Task DeleteUserAsync(int id);
    }
}
