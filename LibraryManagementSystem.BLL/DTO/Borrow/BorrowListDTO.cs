using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Borrow
{
    public class BorrowListDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string BookTitle { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public decimal FineAmount { get; set; }

        public string Status { get; set; }
    }
}
