using CVApp.Data;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;

namespace CVApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<CVDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            builder.Services.AddScoped<ViewRenderService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/CV/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=CV}/{action=Index}/{id?}");

            app.Run();
        }
    }
}