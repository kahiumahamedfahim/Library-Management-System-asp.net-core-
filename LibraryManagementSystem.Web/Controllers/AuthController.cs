using App.ViewModels.Auth;
using AutoMapper;
using BLL.DTO.Auth;
using BLL.Interfaces;
using DAL.EF.Table;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using App.ViewModels.Auth;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        private readonly IMapper _mapper;


        public AuthController(
            IAuthService authService,
            IMapper mapper)
        {
            _authService = authService;

            _mapper = mapper;
        }


        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


       
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                // VM → DTO
                var dto =
                    _mapper.Map<RegisterDTO>(vm);

                await _authService.RegisterAsync(dto);

                TempData["Success"] =
                    "Registration successful. Please login.";

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(vm);
            }
        }


        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


       
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                
                var dto =
                    _mapper.Map<LoginDTO>(vm);

                var user =
                    await _authService.LoginAsync(dto);

                if (user == null)
                {
                    throw new Exception(
                        "Invalid email or password.");
                }


                
                var claims = new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.Id.ToString()),

                    new Claim(
                        ClaimTypes.Name,
                        user.Name),

                    new Claim(
                        ClaimTypes.Email,
                        user.Email),

                    new Claim(
                        ClaimTypes.Role,
                        user.Role)
                };


                
                var claimsIdentity =
                    new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults
                        .AuthenticationScheme);


               
                var principal =
                    new ClaimsPrincipal(
                        claimsIdentity);


                
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults
                    .AuthenticationScheme,
                    principal);


                TempData["Success"] =
                    "Login successful.";

                return RedirectToAction(
                    "Index",
                    "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(vm);
            }
        }


        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                .AuthenticationScheme);

            TempData["Success"] =
                "Logout successful.";

            return RedirectToAction(
                nameof(Login));
        }


        
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}