using AutoMapper;
using BLL.DTO.Auth;
using BLL.DTO.Author;
using BLL.DTO.Author;
using BLL.DTO.Book;
using BLL.DTO.Book;
using BLL.DTO.Borrow;
using BLL.DTO.Borrow;
using BLL.DTO.Catagory;
using BLL.DTO.Catagory;
using BLL.DTO.User;
using BLL.DTO.User;
using DAL.EF.Table;
using BLL.DTO.Auth;


namespace BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Book, BookListDTO>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author.Name))

                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));



            CreateMap<Book, BookDetailsDTO>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author.Name))

                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));



            CreateMap<BookCreateDTO, Book>();

            CreateMap<BookUpdateDTO, Book>();



            CreateMap<Author, AuthorListDTO>();

            CreateMap<AuthorCreateDTO, Author>();

            CreateMap<AuthorUpdateDTO, Author>();



            CreateMap<Category, CategoryListDTO>();

            CreateMap<CategoryCreateDTO, Category>();

            CreateMap<CategoryUpdateDTO, Category>();



            CreateMap<User, UserListDTO>();

            CreateMap<User, UserDetailsDTO>();
            CreateMap<AdminCreateUserDTO, User>();
            CreateMap<RegisterDTO, User>();

            CreateMap<UserUpdateDTO, User>();



            CreateMap<BorrowRecord, BorrowListDTO>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.Name))

                .ForMember(dest => dest.BookTitle,
                    opt => opt.MapFrom(src => src.Book.Title));
        }
    }
}