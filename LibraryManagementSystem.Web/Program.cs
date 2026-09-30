using AutoMapper;
using BLL.Interfaces;
using BLL.Mappings;
using BLL.Services;
using DAL.EF;
using DAL.EF;
using DAL.Interfaces;
using DAL.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Mappings;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
//session cookies
builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)

    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";

        options.AccessDeniedPath =
            "/Auth/AccessDenied";

        options.Cookie.Name =
            "LibraryManagementCookie";

        options.ExpireTimeSpan =
            TimeSpan.FromDays(7);
    });

// Add services to the container.
builder.Services.AddControllersWithViews();


// Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(
    typeof(MappingProfile),
    typeof(WebMappingProfile)
);
//Dependence injection 
builder.Services.AddScoped<IBookRepo, BookRepo>();

builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();

builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();

builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IBorrowRepo, BorrowRepo>();


builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();