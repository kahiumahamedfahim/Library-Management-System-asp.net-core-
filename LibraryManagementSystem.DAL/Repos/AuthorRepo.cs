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
    public  class AuthorRepo : IAuthorRepo
    {
        private readonly AppDbContext _context;

        public AuthorRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var author = await GetByIdAsync(id);

            if (author != null)
            {
                _context.Authors.Remove(author);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

