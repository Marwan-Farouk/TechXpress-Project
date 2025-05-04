using Business.Managers.Brand;
using Business.Managers.Categories;
using Business.Managers.Orders;
using Business.Managers.Products;
using Business.Managers.Users;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Repositories.BRAND;
using DataAccess.Repositories.CATEGORY;
using DataAccess.Repositories.ORDER;
using DataAccess.Repositories.PRODUCT;
using DataAccess.Repositories.USERADDRESS;
using Microsoft.EntityFrameworkCore;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ✅ إضافة دعم الكاش المؤقت للجلسات
            builder.Services.AddDistributedMemoryCache();

            // ✅ تفعيل الجلسة
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // مدة الجلسة
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // ✅ إضافة الخدمات الخاصة بالبزنس
            builder.Services.AddScoped<IProductManager, ProductManager>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryManager, CategoryManager>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IOrderManager, OrderManager>();
            builder.Services.AddScoped<IBrandManager, BrandManager>();
            builder.Services.AddScoped<IAddressManager, AddressManager>();
            builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();


            var connectionString = builder.Configuration.GetConnectionString("TechXpress");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information);
            });

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });


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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
