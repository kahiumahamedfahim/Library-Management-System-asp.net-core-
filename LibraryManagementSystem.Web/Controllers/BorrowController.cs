using DAL.EF;
using DAL.EF.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class BorrowController : Controller
    {
        private readonly AppDbContext _context;

        public BorrowController(AppDbContext context)
        {
            _context = context;
        }


        
        public async Task<IActionResult> MyBorrows()
        {
            var userId =
                int.Parse(User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            var borrows = await _context.BorrowRecords
                .Include(x => x.Book)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.BorrowDate)
                .ToListAsync();

            return View(borrows);
        }


       
        [HttpPost]
        public async Task<IActionResult> BorrowBook(int bookId)
        {
            try
            {
                var userId =
                    int.Parse(User.FindFirstValue(
                        ClaimTypes.NameIdentifier)!);


                var book = await _context.Books
                    .FirstOrDefaultAsync(x => x.Id == bookId);

                if (book == null)
                {
                    TempData["Error"] =
                        "Book not found.";

                    return RedirectToAction(
                        "Index",
                        "Book");
                }


                if (book.Quantity <= 0)
                {
                    TempData["Error"] =
                        "Book out of stock.";

                    return RedirectToAction(
                        "Details",
                        "Book",
                        new { id = bookId });
                }


                
                bool alreadyBorrowed =
                    await _context.BorrowRecords
                    .AnyAsync(x =>
                        x.BookId == bookId &&
                        x.UserId == userId &&
                        x.Status == "Borrowed");

                if (alreadyBorrowed)
                {
                    TempData["Error"] =
                        "You already borrowed this book.";

                    return RedirectToAction(
                        "Details",
                        "Book",
                        new { id = bookId });
                }


                
                var borrow = new BorrowRecord
                {
                    BookId = bookId,
                    UserId = userId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    Status = "Borrowed"
                };


                
                book.Quantity--;


                _context.BorrowRecords.Add(borrow);

                await _context.SaveChangesAsync();


                TempData["Success"] =
                    "Book borrowed successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(
                "MyBorrows");
        }


        
        [HttpPost]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            try
            {
                var borrow =
                    await _context.BorrowRecords
                    .Include(x => x.Book)
                    .FirstOrDefaultAsync(x =>
                        x.Id == borrowId);

                if (borrow == null)
                {
                    TempData["Error"] =
                        "Borrow record not found.";

                    return RedirectToAction(
                        nameof(MyBorrows));
                }


                if (borrow.Status == "Returned")
                {
                    TempData["Error"] =
                        "Book already returned.";

                    return RedirectToAction(
                        nameof(MyBorrows));
                }


                
                borrow.Status = "Returned";

                borrow.ReturnDate = DateTime.Now;

                borrow.Book.Quantity++;


                await _context.SaveChangesAsync();


                TempData["Success"] =
                    "Book returned successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(
                nameof(MyBorrows));
        }


        
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Index()
        {
            var borrows = await _context.BorrowRecords
                .Include(x => x.Book)
                .Include(x => x.User)
                .OrderByDescending(x => x.BorrowDate)
                .ToListAsync();

            return View(borrows);
        }
    }
}