using BLL.DTO.Book;
using DAL.EF.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBookService
    {
        Task<List<BookListDTO>> GetAllBooksAsync();

        Task<BookDetailsDTO?> GetBookByIdAsync(int id);

        Task AddBookAsync(BookCreateDTO dto);

        Task UpdateBookAsync(BookUpdateDTO dto);

        Task DeleteBookAsync(int id);
    }
}
