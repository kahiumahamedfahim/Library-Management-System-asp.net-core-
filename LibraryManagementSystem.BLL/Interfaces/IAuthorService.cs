using BLL.DTO.Author;

namespace BLL.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorListDTO>> GetAllAuthorsAsync();

        Task<AuthorListDTO?> GetAuthorByIdAsync(int id);

        Task AddAuthorAsync(AuthorCreateDTO dto);

        Task UpdateAuthorAsync(AuthorUpdateDTO dto);

        Task DeleteAuthorAsync(int id);
    }
}