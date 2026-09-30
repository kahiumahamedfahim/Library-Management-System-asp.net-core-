using App.ViewModels.Author;
using AutoMapper;
using BLL.DTO.Author;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
       
        
            private readonly IAuthorService _authorService;

            private readonly IMapper _mapper;


            public AuthorController(
                IAuthorService authorService,
                IMapper mapper)
            {
                _authorService = authorService;

                _mapper = mapper;
            }


            
            public async Task<IActionResult> Index()
            {
                var dtos =
                    await _authorService.GetAllAuthorsAsync();

                var vms =
                    _mapper.Map<List<AuthorListVM>>(dtos);

                return View(vms);
            }


            
            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }


           
            [HttpPost]
            public async Task<IActionResult> Create(
                AuthorCreateVM vm)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View(vm);
                    }

                    var dto =
                        _mapper.Map<AuthorCreateDTO>(vm);

                    await _authorService
                        .AddAuthorAsync(dto);

                    TempData["Success"] =
                        "Author created successfully.";

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
                        await _authorService
                        .GetAuthorByIdAsync(id);

                    if (dto == null)
                    {
                        return NotFound();
                    }

                    var vm =
                        _mapper.Map<AuthorEditVM>(dto);

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
                AuthorEditVM vm)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View(vm);
                    }

                    var dto =
                        _mapper.Map<AuthorUpdateDTO>(vm);

                    await _authorService
                        .UpdateAuthorAsync(dto);

                    TempData["Success"] =
                        "Author updated successfully.";

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
                    await _authorService
                        .DeleteAuthorAsync(id);

                    TempData["Success"] =
                        "Author deleted successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }

