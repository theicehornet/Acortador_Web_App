using Acortador_Web_App.Models;
using Microsoft.EntityFrameworkCore;
using Acortador_Web_App.Services.Contract;
using Acortador_Web_App.Services.Implementation;

namespace Acortador_Web_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ShorturlContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ACORTADORDB"));
            });

            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acortador}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
