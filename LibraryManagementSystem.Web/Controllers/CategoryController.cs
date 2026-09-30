using App.ViewModels.Category;
using AutoMapper;
using BLL.DTO.Catagory;

using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.ViewModels.Category;

namespace Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        private readonly IMapper _mapper;


        public CategoryController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;

            _mapper = mapper;
        }


        
        public async Task<IActionResult> Index()
        {
            var dtos =
                await _categoryService.GetAllCategoriesAsync();

            var vms =
                _mapper.Map<List<CategoryListVM>>(dtos);

            return View(vms);
        }


        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


       
        [HttpPost]
        public async Task<IActionResult> Create(
            CategoryCreateVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var dto =
                    _mapper.Map<CategoryCreateDTO>(vm);

                await _categoryService
                    .AddCategoryAsync(dto);

                TempData["Success"] =
                    "Category created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(vm);
            }
        }


       
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto =
                    await _categoryService
                    .GetCategoryByIdAsync(id);

                if (dto == null)
                {
                    return NotFound();
                }

                var vm =
                    _mapper.Map<CategoryEditVM>(dto);

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }


        
        [HttpPost]
        public async Task<IActionResult> Edit(
            CategoryEditVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var dto =
                    _mapper.Map<CategoryUpdateDTO>(vm);

                await _categoryService
                    .UpdateCategoryAsync(dto);

                TempData["Success"] =
                    "Category updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(vm);
            }
        }


        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService
                    .DeleteCategoryAsync(id);

                TempData["Success"] =
                    "Category deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}