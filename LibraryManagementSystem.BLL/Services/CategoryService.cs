using AutoMapper;
using BLL.DTO.Catagory;

using BLL.Interfaces;
using DAL.EF.Table;
using DAL.Interfaces;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;

        private readonly IBookRepo _bookRepo;

        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepo categoryRepo,
            IBookRepo bookRepo,
            IMapper mapper)
        {
            _categoryRepo = categoryRepo;

            _bookRepo = bookRepo;

            _mapper = mapper;
        }


        public async Task<List<CategoryListDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return _mapper.Map<List<CategoryListDTO>>(categories);
        }


        public async Task<CategoryListDTO?> GetCategoryByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid category id.");
            }

            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            return _mapper.Map<CategoryListDTO>(category);
        }



        public async Task AddCategoryAsync(CategoryCreateDTO dto)
        {
            
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name is required.");
            }

            if (dto.Name.Length < 2)
            {
                throw new Exception("Category name is too short.");
            }

            if (dto.Name.Length > 100)
            {
                throw new Exception("Category name is too long.");
            }


          
            var categories = await _categoryRepo.GetAllAsync();

            bool exists = categories.Any(c =>
                c.Name.Trim().ToLower() ==
                dto.Name.Trim().ToLower());

            if (exists)
            {
                throw new Exception("Category already exists.");
            }


            
            var category = _mapper.Map<Category>(dto);


            
            await _categoryRepo.AddAsync(category);

            await _categoryRepo.SaveAsync();
        }


        
        public async Task UpdateCategoryAsync(CategoryUpdateDTO dto)
        {
            if (dto.Id <= 0)
            {
                throw new Exception("Invalid category id.");
            }

            var existingCategory =
                await _categoryRepo.GetByIdAsync(dto.Id);

            if (existingCategory == null)
            {
                throw new Exception("Category not found.");
            }


            
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception("Category name is required.");
            }

            if (dto.Name.Length < 2)
            {
                throw new Exception("Category name is too short.");
            }


            
            var categories = await _categoryRepo.GetAllAsync();

            bool duplicateExists = categories.Any(c =>
                c.Id != dto.Id &&
                c.Name.Trim().ToLower() ==
                dto.Name.Trim().ToLower());

            if (duplicateExists)
            {
                throw new Exception("Another category with same name already exists.");
            }


           
            _mapper.Map(dto, existingCategory);


            
            await _categoryRepo.UpdateAsync(existingCategory);

            await _categoryRepo.SaveAsync();
        }


        
        public async Task DeleteCategoryAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid category id.");
            }

            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                throw new Exception("Category not found.");
            }


            
            var books = await _bookRepo.GetAllAsync();

            bool hasBooks = books.Any(b => b.CategoryId == id);

            if (hasBooks)
            {
                throw new Exception("Cannot delete category with existing books.");
            }


            
            await _categoryRepo.DeleteAsync(id);

            await _categoryRepo.SaveAsync();
        }
    }
}