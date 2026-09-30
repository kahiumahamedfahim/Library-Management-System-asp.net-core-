using BLL.DTO.Borrow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBorrowService
    {
        Task<List<BorrowListDTO>> GetAllBorrowsAsync();

        Task BorrowBookAsync(BorrowCreateDTO dto);

        Task ReturnBookAsync(ReturnBookDTO dto);

        Task DeleteBorrowAsync(int id);
    }
}
