using DAL.EF.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public  interface IBorrowRepo
    {
        Task<List<BorrowRecord>> GetAllAsync();

        Task<BorrowRecord?> GetByIdAsync(int id);

        Task<List<BorrowRecord>> GetBorrowDetailsAsync();

        Task AddAsync(BorrowRecord borrowRecord);

        Task UpdateAsync(BorrowRecord borrowRecord);

        Task DeleteAsync(int id);

        Task SaveAsync();
    }
}
