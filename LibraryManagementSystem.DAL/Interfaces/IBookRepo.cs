using DAL.EF.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public  interface IBookRepo
    {
        Task<List<Book>> GetAllAsync();

        Task<Book?> GetByIdAsync(int id);

        Task AddAsync(Book book);

        Task UpdateAsync(Book book);

        Task DeleteAsync(int id);

        Task SaveAsync();

        Task<List<Book>> GetBooksWithDetailsAsync();
    }
}
