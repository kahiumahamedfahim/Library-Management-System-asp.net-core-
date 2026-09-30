using AutoMapper;
using BLL.DTO.Book;
using BLL.Interfaces;
using DAL.EF.Table;
using DAL.Interfaces;

namespace BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;

        private readonly IAuthorRepo _authorRepo;

        private readonly ICategoryRepo _categoryRepo;

        private readonly IBorrowRepo _borrowRepo;

        private readonly IMapper _mapper;

        public BookService(
            IBookRepo bookRepo,
            IAuthorRepo authorRepo,
            ICategoryRepo categoryRepo,
            IBorrowRepo borrowRepo,
            IMapper mapper)
        {
            _bookRepo = bookRepo;

            _authorRepo = authorRepo;

            _categoryRepo = categoryRepo;

            _borrowRepo = borrowRepo;

            _mapper = mapper;
        }


        public async Task<List<BookListDTO>> GetAllBooksAsync()
        {
            var books = await _bookRepo.GetBooksWithDetailsAsync();

            return _mapper.Map<List<BookListDTO>>(books);
        }


        public async Task<BookDetailsDTO?> GetBookByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid book id.");
            }

            var book = await _bookRepo.GetByIdAsync(id);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }

            return _mapper.Map<BookDetailsDTO>(book);
        }


        public async Task AddBookAsync(BookCreateDTO dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Book title is required.");
            }

            if (dto.Title.Length < 2)
            {
                throw new Exception("Book title is too short.");
            }

            if (dto.Title.Length > 200)
            {
                throw new Exception("Book title is too long.");
            }


            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                if (dto.Description.Length > 2000)
                {
                    throw new Exception("Description is too long.");
                }
            }



            if (dto.Quantity < 0)
            {
                throw new Exception("Quantity cannot be negative.");
            }

            if (dto.Quantity > 1000)
            {
                throw new Exception("Quantity is too large.");
            }



            if (dto.AuthorId <= 0)
            {
                throw new Exception("Author is required.");
            }

            var author = await _authorRepo.GetByIdAsync(dto.AuthorId);

            if (author == null)
            {
                throw new Exception("Selected author does not exist.");
            }



            if (dto.CategoryId <= 0)
            {
                throw new Exception("Category is required.");
            }

            var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                throw new Exception("Selected category does not exist.");
            }



            var books = await _bookRepo.GetAllAsync();

            bool exists = books.Any(b =>
                b.Title.Trim().ToLower() ==
                dto.Title.Trim().ToLower());

            if (exists)
            {
                throw new Exception("Book already exists.");
            }


            if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                bool validImage =
                    dto.ImageUrl.EndsWith(".jpg") ||
                    dto.ImageUrl.EndsWith(".jpeg") ||
                    dto.ImageUrl.EndsWith(".png") ||
                    dto.ImageUrl.EndsWith(".webp");

                if (!validImage)
                {
                    throw new Exception("Invalid image format.");
                }
            }



            var book = _mapper.Map<Book>(dto);


            book.CreatedAt = DateTime.Now;


            await _bookRepo.AddAsync(book);

            await _bookRepo.SaveAsync();
        }



        public async Task UpdateBookAsync(BookUpdateDTO dto)
        {
            if (dto.Id <= 0)
            {
                throw new Exception("Invalid book id.");
            }



            var existingBook = await _bookRepo.GetByIdAsync(dto.Id);

            if (existingBook == null)
            {
                throw new Exception("Book not found.");
            }



            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Book title is required.");
            }

            if (dto.Title.Length < 2)
            {
                throw new Exception("Book title is too short.");
            }


            if (dto.Quantity < 0)
            {
                throw new Exception("Quantity cannot be negative.");
            }



            var author = await _authorRepo.GetByIdAsync(dto.AuthorId);

            if (author == null)
            {
                throw new Exception("Selected author does not exist.");
            }



            var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                throw new Exception("Selected category does not exist.");
            }



            var books = await _bookRepo.GetAllAsync();

            bool duplicateExists = books.Any(b =>
                b.Id != dto.Id &&
                b.Title.Trim().ToLower() ==
                dto.Title.Trim().ToLower());

            if (duplicateExists)
            {
                throw new Exception("Another book with same title already exists.");
            }



            var createdDate = existingBook.CreatedAt;



            _mapper.Map(dto, existingBook);

            existingBook.CreatedAt = createdDate;



            await _bookRepo.UpdateAsync(existingBook);

            await _bookRepo.SaveAsync();
        }



        public async Task DeleteBookAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid book id.");
            }



            var existingBook = await _bookRepo.GetByIdAsync(id);

            if (existingBook == null)
            {
                throw new Exception("Book not found.");
            }



            var borrows = await _borrowRepo.GetAllAsync();

            bool isBorrowed = borrows.Any(b =>
                b.BookId == id &&
                b.Status != "Returned");

            if (isBorrowed)
            {
                throw new Exception("Cannot delete borrowed book.");
            }



            await _bookRepo.DeleteAsync(id);

            await _bookRepo.SaveAsync();
        }
    }
}