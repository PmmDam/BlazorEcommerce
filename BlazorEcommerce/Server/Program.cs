global using BlazorEcommerce.Shared;
global using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Server.Data;
using Microsoft.AspNetCore.ResponseCompression;

namespace BlazorEcommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Añadimos el contecto de nuestra base de datos y nos conectamos con la cadena de conexión por defecto almacenada en appsettings.json
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //Añadimos las dependencias de Swagger para poder añadir una interfaz de usuario a nuestras APIs
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //Le decimos a la aplicación que utilice la UI de Swagger
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Le decimos a la aplicación que utilice Swagger
            app.UseSwagger();


            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}