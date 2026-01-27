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
            //1
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
            //1
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
            // Hala (67-81)
            //1
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
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

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
