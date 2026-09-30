using AutoMapper;
using BLL.DTO.Borrow;
using BLL.Interfaces;
using DAL.EF.Table;
using DAL.Interfaces;

namespace BLL.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepo _borrowRepo;

        private readonly IUserRepo _userRepo;

        private readonly IBookRepo _bookRepo;

        private readonly IMapper _mapper;

        public BorrowService(
            IBorrowRepo borrowRepo,
            IUserRepo userRepo,
            IBookRepo bookRepo,
            IMapper mapper)
        {
            _borrowRepo = borrowRepo;

            _userRepo = userRepo;

            _bookRepo = bookRepo;

            _mapper = mapper;
        }



        public async Task<List<BorrowListDTO>> GetAllBorrowsAsync()
        {
            var borrows = await _borrowRepo.GetBorrowDetailsAsync();

            return _mapper.Map<List<BorrowListDTO>>(borrows);
        }



        public async Task BorrowBookAsync(BorrowCreateDTO dto)
        {

            var user = await _userRepo.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }



            var book = await _bookRepo.GetByIdAsync(dto.BookId);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }



            if (book.Quantity <= 0)
            {
                throw new Exception("Book is unavailable.");
            }



            var borrows = await _borrowRepo.GetAllAsync();

            bool alreadyBorrowed = borrows.Any(b =>
                b.UserId == dto.UserId &&
                b.BookId == dto.BookId &&
                b.Status != "Returned");

            if (alreadyBorrowed)
            {
                throw new Exception("User already borrowed this book.");
            }



            book.Quantity--;

            await _bookRepo.UpdateAsync(book);



            var borrow = new BorrowRecord
            {
                UserId = dto.UserId,

                BookId = dto.BookId,

                BorrowDate = DateTime.Now,

                DueDate = DateTime.Now.AddDays(7),

                Status = "Borrowed",

                FineAmount = 0
            };


            await _borrowRepo.AddAsync(borrow);

            await _bookRepo.SaveAsync();

            await _borrowRepo.SaveAsync();
        }



        public async Task ReturnBookAsync(ReturnBookDTO dto)
        {
            var borrow = await _borrowRepo.GetByIdAsync(dto.BorrowId);

            if (borrow == null)
            {
                throw new Exception("Borrow record not found.");
            }



            if (borrow.Status == "Returned")
            {
                throw new Exception("Book already returned.");
            }



            var book = await _bookRepo.GetByIdAsync(borrow.BookId);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }



            book.Quantity++;

            await _bookRepo.UpdateAsync(book);



            borrow.ReturnDate = DateTime.Now;

            borrow.Status = "Returned";



            if (DateTime.Now > borrow.DueDate)
            {
                int lateDays =
                    (DateTime.Now - borrow.DueDate).Days;

                borrow.FineAmount = lateDays * 10;
            }


            await _borrowRepo.UpdateAsync(borrow);

            await _bookRepo.SaveAsync();

            await _borrowRepo.SaveAsync();
        }



        public async Task DeleteBorrowAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid borrow id.");
            }

            var borrow = await _borrowRepo.GetByIdAsync(id);

            if (borrow == null)
            {
                throw new Exception("Borrow record not found.");
            }


 
            if (borrow.Status != "Returned")
            {
                throw new Exception("Cannot delete active borrow record.");
            }

            await _borrowRepo.DeleteAsync(id);

            await _borrowRepo.SaveAsync();
        }
    }
}