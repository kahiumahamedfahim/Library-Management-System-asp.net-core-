using DAL.EF.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public  interface IUserRepo
    {
        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);

        Task SaveAsync();
    }
}
