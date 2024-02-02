using Acortador_Web_App.Models;
using Microsoft.EntityFrameworkCore;
using Acortador_Web_App.Services;

namespace Acortador_Web_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AcortadorurlContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ACORTADORDB"));
            });

            builder.Services.AddScoped<IEmailService,EmailService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acortador}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
