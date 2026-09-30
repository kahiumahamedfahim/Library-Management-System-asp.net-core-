using DAL.EF;
using DAL.EF.Table;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class BookRepo : IBookRepo
    {
        private readonly AppDbContext _context;

        public BookRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);

            if (book != null)
            {
                _context.Books.Remove(book);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetBooksWithDetailsAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }
    }
}
