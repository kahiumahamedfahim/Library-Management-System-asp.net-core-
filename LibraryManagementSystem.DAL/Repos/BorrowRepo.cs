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
    public  class BorrowRepo : IBorrowRepo
    {
        private readonly AppDbContext _context;

        public BorrowRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BorrowRecord>> GetAllAsync()
        {
            return await _context.BorrowRecords.ToListAsync();
        }

        public async Task<BorrowRecord?> GetByIdAsync(int id)
        {
            return await _context.BorrowRecords.FindAsync(id);
        }

        public async Task<List<BorrowRecord>> GetBorrowDetailsAsync()
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task AddAsync(BorrowRecord borrowRecord)
        {
            await _context.BorrowRecords.AddAsync(borrowRecord);
        }

        public async Task UpdateAsync(BorrowRecord borrowRecord)
        {
            _context.BorrowRecords.Update(borrowRecord);

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var borrowRecord = await GetByIdAsync(id);

            if (borrowRecord != null)
            {
                _context.BorrowRecords.Remove(borrowRecord);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
