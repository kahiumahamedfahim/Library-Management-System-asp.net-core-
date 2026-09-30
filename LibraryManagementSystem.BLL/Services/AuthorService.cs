using AutoMapper;
using BLL.DTO.Author;
using BLL.Interfaces;
using DAL.EF.Table;
using DAL.Interfaces;

namespace BLL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepo _authorRepo;

        private readonly IBookRepo _bookRepo;

        private readonly IMapper _mapper;

        public AuthorService(
            IAuthorRepo authorRepo,
            IBookRepo bookRepo,
            IMapper mapper)
        {
            _authorRepo = authorRepo;

            _bookRepo = bookRepo;

            _mapper = mapper;
        }


        public async Task<List<AuthorListDTO>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepo.GetAllAsync();

            return _mapper.Map<List<AuthorListDTO>>(authors);
        }

        public async Task<AuthorListDTO?> GetAuthorByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid author id.");
            }

            var author = await _authorRepo.GetByIdAsync(id);

            if (author == null)
            {
                throw new Exception("Author not found.");
            }

            return _mapper.Map<AuthorListDTO>(author);
        }

        public async Task AddAuthorAsync(AuthorCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Author name is required.");
            }

            if (dto.Name.Length < 2)
            {
                throw new Exception("Author name is too short.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Bio))
            {
                if (dto.Bio.Length > 3000)
                {
                    throw new Exception("Author bio is too long.");
                }
            }

            var authors = await _authorRepo.GetAllAsync();

            bool exists = authors.Any(a =>
                a.Name.Trim().ToLower() ==
                dto.Name.Trim().ToLower());

            if (exists)
            {
                throw new Exception("Author already exists.");
            }

            var author = _mapper.Map<Author>(dto);


            await _authorRepo.AddAsync(author);

            await _authorRepo.SaveAsync();
        }


        public async Task UpdateAuthorAsync(AuthorUpdateDTO dto)
        {
            if (dto.Id <= 0)
            {
                throw new Exception("Invalid author id.");
            }

            var existingAuthor =
                await _authorRepo.GetByIdAsync(dto.Id);

            if (existingAuthor == null)
            {
                throw new Exception("Author not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Author name is required.");
            }

          
            var authors = await _authorRepo.GetAllAsync();

            bool duplicateExists = authors.Any(a =>
                a.Id != dto.Id &&
                a.Name.Trim().ToLower() ==
                dto.Name.Trim().ToLower());

            if (duplicateExists)
            {
                throw new Exception("Another author with same name already exists.");
            }

           
            _mapper.Map(dto, existingAuthor);

            await _authorRepo.UpdateAsync(existingAuthor);

            await _authorRepo.SaveAsync();
        }



        public async Task DeleteAuthorAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid author id.");
            }

            var author = await _authorRepo.GetByIdAsync(id);

            if (author == null)
            {
                throw new Exception("Author not found.");
            }

            var books = await _bookRepo.GetAllAsync();

            bool hasBooks = books.Any(b => b.AuthorId == id);

            if (hasBooks)
            {
                throw new Exception("Cannot delete author with existing books.");
            }


            await _authorRepo.DeleteAsync(id);

            await _authorRepo.SaveAsync();
        }
    }
}