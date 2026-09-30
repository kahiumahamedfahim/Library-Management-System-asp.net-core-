using BLL.DTO.Catagory;
using DAL.EF.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryListDTO>> GetAllCategoriesAsync();

        Task<CategoryListDTO?> GetCategoryByIdAsync(int id);

        Task AddCategoryAsync(CategoryCreateDTO dto);

        Task UpdateCategoryAsync(CategoryUpdateDTO dto);

        Task DeleteCategoryAsync(int id);
    }
}
