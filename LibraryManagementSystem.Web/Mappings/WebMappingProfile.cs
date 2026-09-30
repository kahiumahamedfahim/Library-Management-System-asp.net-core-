using AutoMapper;

using BLL.DTO.Book;

using Web.ViewModels.Book;
using App.ViewModels.Auth;

using BLL.DTO.Auth;
using App.ViewModels.Category;

using BLL.DTO.Catagory;

using App.ViewModels.Author;

using BLL.DTO.Author;

namespace Web.Mappings
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            // =========================
            // BOOK
            // =========================

            CreateMap<BookCreateVM, BookCreateDTO>();

            CreateMap<BookEditVM, BookUpdateDTO>();


            CreateMap<BookDetailsDTO, BookDetailsVM>();

            CreateMap<BookListDTO, BookListVM>();


            CreateMap<BookDetailsDTO, BookEditVM>();
            CreateMap<LoginVM, LoginDTO>();

            CreateMap<RegisterVM, RegisterDTO>();
            CreateMap<CategoryCreateVM, CategoryCreateDTO>();

            CreateMap<CategoryEditVM, CategoryUpdateDTO>();

            CreateMap<CategoryListDTO, CategoryListVM>();

            CreateMap<CategoryListDTO, CategoryEditVM>();
            CreateMap<AuthorCreateVM, AuthorCreateDTO>();

            CreateMap<AuthorEditVM, AuthorUpdateDTO>();

            CreateMap<AuthorListDTO, AuthorListVM>();

            CreateMap<AuthorListDTO, AuthorEditVM>();
        }
    }
}