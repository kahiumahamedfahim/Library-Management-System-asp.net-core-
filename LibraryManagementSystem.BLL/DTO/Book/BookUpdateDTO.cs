using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Book
{
    public class BookUpdateDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }
    }
}
