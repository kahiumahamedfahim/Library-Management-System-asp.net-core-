using BLL.DTO.Auth;
using DAL.EF.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO dto);

        Task<User?> LoginAsync(LoginDTO dto);

        Task CreateUserAsync(
    string name,
    string email,
    string password,
    string role);
    }
}
