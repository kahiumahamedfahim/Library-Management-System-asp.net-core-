using App.ViewModels.User;
using BLL.Interfaces;
using DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.ViewModels.User;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IAuthService _authService;


        public UserController(
            AppDbContext context,
            IAuthService authService)
        {
            _context = context;

            _authService = authService;
        }


        
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderBy(x => x.Role)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return View(users);
        }


        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateUserVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                await _authService.CreateUserAsync(
                    vm.Name,
                    vm.Email,
                    vm.Password,
                    vm.Role);

                TempData["Success"] =
                    "User created successfully.";

                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(vm);
            }
        }
    }
}