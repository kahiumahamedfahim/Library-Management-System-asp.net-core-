using AutoMapper;
using BLL.DTO.Book;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModels.Book;

namespace Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        private readonly IAuthorService _authorService;

        private readonly ICategoryService _categoryService;

        private readonly IWebHostEnvironment _environment;

        private readonly IMapper _mapper;


        public BookController(
            IBookService bookService,
            IAuthorService authorService,
            ICategoryService categoryService,
            IWebHostEnvironment environment,
            IMapper mapper)
        {
            _bookService = bookService;

            _authorService = authorService;

            _categoryService = categoryService;

            _environment = environment;

            _mapper = mapper;
        }


        
        public async Task<IActionResult> Index()
        {
            var bookDtos =
                await _bookService.GetAllBooksAsync();

            var vms =
                _mapper.Map<List<BookListVM>>(bookDtos);

            return View(vms);
        }


        
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var dto =
                    await _bookService.GetBookByIdAsync(id);

                if (dto == null)
                {
                    return NotFound();
                }

                var vm =
                    _mapper.Map<BookDetailsVM>(dto);

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }


        
        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new BookCreateVM();

            await LoadDropdowns(vm);

            return View(vm);
        }


        
        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateVM vm)
        {
            try
            {
                await LoadDropdowns(vm);

                if (!ModelState.IsValid)
                {
                    return View(vm);
                }


                
                string? imagePath = null;

                if (vm.ImageFile != null)
                {
                    string folder =
                        Path.Combine(
                            _environment.WebRootPath,
                            "uploads/books");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string fileName =
                        Guid.NewGuid().ToString()
                        + Path.GetExtension(vm.ImageFile.FileName);

                    string filePath =
                        Path.Combine(folder, fileName);

                    using (var stream =
                           new FileStream(filePath, FileMode.Create))
                    {
                        await vm.ImageFile.CopyToAsync(stream);
                    }

                    imagePath = "/uploads/books/" + fileName;
                }


                
                var dto =
                    _mapper.Map<BookCreateDTO>(vm);

                dto.ImageUrl = imagePath;


                await _bookService.AddBookAsync(dto);

                TempData["Success"] =
                    "Book created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(vm);
            }
        }


        
        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto =
                    await _bookService.GetBookByIdAsync(id);

                if (dto == null)
                {
                    return NotFound();
                }

                var vm =
                    _mapper.Map<BookEditVM>(dto);

                vm.ExistingImageUrl = dto.ImageUrl;

                await LoadDropdowns(vm);

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }


        
        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
      
        public async Task<IActionResult> Edit(BookEditVM vm)
        {
            try
            {
                await LoadDropdowns(vm);

                if (!ModelState.IsValid)
                {
                    return View(vm);
                }


                string? imagePath =
                    vm.ExistingImageUrl;


                
                if (vm.ImageFile != null)
                {
                    string folder =
                        Path.Combine(
                            _environment.WebRootPath,
                            "uploads/books");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string fileName =
                        Guid.NewGuid().ToString()
                        + Path.GetExtension(vm.ImageFile.FileName);

                    string filePath =
                        Path.Combine(folder, fileName);

                    using (var stream =
                           new FileStream(filePath, FileMode.Create))
                    {
                        await vm.ImageFile.CopyToAsync(stream);
                    }

                    imagePath = "/uploads/books/" + fileName;
                }


                
                var dto =
                    _mapper.Map<BookUpdateDTO>(vm);

                dto.ImageUrl = imagePath;


                await _bookService.UpdateBookAsync(dto);

                TempData["Success"] =
                    "Book updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(vm);
            }
        }


        
        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);

                TempData["Success"] =
                    "Book deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


        
        private async Task LoadDropdowns(BookCreateVM vm)
        {
            var authors =
                await _authorService.GetAllAuthorsAsync();

            var categories =
                await _categoryService.GetAllCategoriesAsync();

            vm.Authors = authors.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();

            vm.Categories = categories.Select(c =>
                new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
        }


        
        private async Task LoadDropdowns(BookEditVM vm)
        {
            var authors =
                await _authorService.GetAllAuthorsAsync();

            var categories =
                await _categoryService.GetAllCategoriesAsync();

            vm.Authors = authors.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();

            vm.Categories = categories.Select(c =>
                new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
        }
    }
}