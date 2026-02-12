using E_Commerce.IRepository;
using E_Commerce.Models;
using E_Commerce.Reposiory;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            // Sara (19-33)
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            //2
            //3
            //4
            //5
            //6
            //7
            //8
            //9
            //10
            //11
            //12
            //13
            //14
            //15
            // Rania (35-49)
            //1
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ECommerceDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            
            
            

            //2
            //3
            //4
            //5
            //6
            //7
            //8
            //9
            //10
            //11
            //12
            //13
            //14
            //15
            // Arwa (51-65)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            //3
            //4
            //5
            //6
            //7
            //8
            //9
            //10
            //11
            //12
            //13
            //14
            //15
            // Hala (67-81)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
			builder.Services.AddHttpClient<IPaymobRepository, PaymobRepository>();
			builder.Services.AddScoped<PaymobRepository>();
			builder.Services.AddHttpClient<PaymobRepository>();

			var app = builder.Build();
            app.UseSession();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            // Rania🤨
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
